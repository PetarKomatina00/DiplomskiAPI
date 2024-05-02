using Azure.Storage.Blobs;
using DiplomskiAPI.Data;
using DiplomskiAPI.Model;
using DiplomskiAPI.Model.DTO;
using DiplomskiAPI.Services;
using DiplomskiAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace DiplomskiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LekController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlobService _blobService;
        private APIResponse _response;
        public LekController(
            ApplicationDbContext context,
            APIResponse response,
            IBlobService blobService)

        {
            _context = context;
            _response = response;
            _blobService = blobService;
        }

        [HttpGet("AllCategory")]
        public async Task<ActionResult> GetAllCategories()
        {
            var categoryList = _context.Lekovi.Select(x => x.MainCategory).ToList();
            return Ok(categoryList);
        }

        [HttpGet("GetImageURL/{blobName}/{containerName}")]
        public async Task<ActionResult<APIResponse>> GetImageURL(string blobName, string containerName = "reklamee")
        {
            return Ok(_blobService.GetBlob(blobName, containerName));
        }

        [HttpGet("GetAllBlobURL")]
        public async Task<ActionResult<APIResponse>> GetAllBlobURL()
        {
            //string folderPath = @"C:\Users\Petar\Desktop\GitHub\Diplomski\my-app\src\Assets\Images";
            //string[] files = Directory.GetFiles(folderPath);
            //string[] ImageName = new string[files.Length];
            //for (int i = 0; i < files.Length; i ++)
            //{
            //    int pathLength = (files[i].Split('\\')).Length;
            //    ImageName[i] = files[i].Split('\\')[pathLength - 1];
            //}

            //foreach(var x in ImageName)
            //{

            //}

            return Ok(_blobService.GetAllBlobs(SD.AzureAddsStorageContainer));      
        }
        [HttpPost("UploadImageAdd")]
        public async Task<ActionResult<APIResponse>> UploadImageAdd(IFormFile image)
        {
            string FileExtensionName = $"{Path.GetExtension(image.FileName)}";
            try
            {
                var x = _blobService.UploadBlob(image.FileName, SD.AzureAddsStorageContainer, image);
                return Ok(x);
            }
            catch(Exception e)
            {
                _response.IsSuccess = false;
                _response.Result = e;
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> getLekovi(int currentPage = 1, int pageSize = 2, bool bestsellers = false)
        {
            if (!bestsellers)
            {
                _response.Result = _context.Lekovi.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                _response.Result = _context.Lekovi.Skip((currentPage - 1) * pageSize).Take(pageSize).Where(x => x.BestSeller).ToList();
            }
            if (_response.Result != null)
            {
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
            }

            Pagination p = new Pagination
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalRecords = _context.Lekovi.Count()
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(p));
            return Ok(_response);
        }
        [HttpGet("{currentPage:int}/{size:int}/{searchString}")]
        public async Task<ActionResult<APIResponse>> getLekovi2(int currentPage = 1, int size = 2, string searchString = "")
        {
            if (searchString != "")
            {
                _response.Result = _context.Lekovi.Where(x => x.NazivLeka.ToLower().Contains(searchString.ToLower())).ToList();
            }
            if (_response.Result != null)
            {
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "getSpecificLek")]
        public async Task<IActionResult> getSpecificLek(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            Lek lek = _context.Lekovi.FirstOrDefault(x => x.LekID == id);
            _response.Result = lek;
            if (lek == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("GetBestSellers")]
        public async Task<ActionResult<APIResponse>> GetBestSellers()
        {
            //Staticni bestselleri ih ima isto 5. 
            var dynamicBestSellers = _context.Lekovi.OrderByDescending(x => x.TimesBought).Take(6).ToList();
            //var staticBestSellers = _context.Lekovi.OrderByDescending(x => x.TimesBought).Where(x => x.BestSeller == true).ToList();


            //if (dynamicBestSellers != null)
            //{
            //    for(int i = 0; i < dynamicBestSellers.Count(); ++i)
            //    {
            //        if (dynamicBestSellers[i].TimesBought > 10)
            //        {
            //            staticBestSellers[dynamicBestSellers.Count() - 1 - i] = dynamicBestSellers[i];
            //        }
            //    }
            //}
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = dynamicBestSellers;
            return Ok(_response);
        }



        [HttpPost("Add")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> CreateLek([FromForm] LekCreateDTO lekDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (lekDTO.Image == null || lekDTO.Image.Length == 0)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }
                    Lek lekIzBaze = _context.Lekovi.FirstOrDefault(x => x.ISBN == lekDTO.ISBN);
                    if (lekIzBaze != null)
                    {
                        _response.ErrorMessages.Add("Lek s datim ISBN-om vec postoji!!");
                        _response.IsSuccess = false;
                        return _response;
                    }

                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(lekDTO.Image.FileName)}";
                    Lek lek = new Lek()
                    {
                        NazivLeka = lekDTO.NazivLeka,
                        Description = lekDTO.Description,
                        ISBN = lekDTO.ISBN,
                        Price = lekDTO.Price,
                        MainCategory = lekDTO.MainCategory,
                        BestSeller = lekDTO.BestSeller,
                        TimesBought = lekDTO.TimesBought,
                        Image = (_blobService.UploadBlob(fileName, SD.AzureStorageContainer, lekDTO.Image))
                    };
                    _context.Lekovi.Add(lek);
                    _context.SaveChanges();
                    _response.Result = lek;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("getSpecificLek", new { id = lek.LekID }, _response);
                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message.ToString());
            }
            return _response;
        }

        [ActionName("Update")]
        [HttpPut("{id:int}", Name = "Update")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdateLek(int id, [FromForm] LekUpdateDTO lekDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (lekDTO == null || id != lekDTO.LekID)
                    {
                        return BadRequest();
                    }

                    Lek lekizBaze = await _context.Lekovi.FindAsync(id);
                    if (lekizBaze == null)
                    {
                        return BadRequest();
                    }

                    lekizBaze.NazivLeka = lekDTO.NazivLeka;
                    lekizBaze.ISBN = lekDTO.ISBN;
                    lekizBaze.Description = lekDTO.Description;
                    lekizBaze.Price = lekDTO.Price;
                    if (lekDTO.Image != null && lekDTO.Image.Length > 0)
                    {
                        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(lekDTO.Image.FileName)}";
                        await _blobService.DeleteBlob(lekizBaze.Image.Split('/').Last(), SD.AzureStorageContainer);
                        lekizBaze.Image = (_blobService.UploadBlob(fileName, SD.AzureStorageContainer, lekDTO.Image));
                    }


                    _context.Lekovi.Update(lekizBaze);
                    _context.SaveChanges();
                    _response.Result = lekizBaze;
                    _response.StatusCode = HttpStatusCode.NoContent;
                    _response.IsSuccess = true;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message.ToString());
            }
            return _response;
        }


        [HttpPut("UpdateTimesBought")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdateTimesBought([FromBody] List<LekUpdateTimesBoughtDTO> DTO)
        {
            try
            {

                foreach(var lek in DTO)
                {
                    var specificLekFromDB = _context.Lekovi.Where(x => x.LekID == lek.LekID).FirstOrDefault();
                    if(specificLekFromDB != null)
                    {
                        specificLekFromDB.TimesBought += lek.Kolicina;
                        _context.Lekovi.Update(specificLekFromDB);  
                    }
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _response.ErrorMessages.Add(ex.Message);
            }
            return _response;
        }
        [ActionName("Delete")]
        [HttpDelete("{id:int}", Name = "Delete")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteLek(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                Lek lekizBaze = await _context.Lekovi.FindAsync(id);
                if (lekizBaze == null)
                {
                    return BadRequest();
                }

                await _blobService.DeleteBlob(lekizBaze.Image.Split('/').Last(), SD.AzureStorageContainer);
                Thread.Sleep(2000);
                _context.Lekovi.Remove(lekizBaze);
                _context.SaveChanges();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message.ToString());
            }
            return _response;
        }



        
    }
}
