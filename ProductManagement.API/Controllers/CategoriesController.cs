using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductManagement.API.Dtos;
using ProductManagement.Core.Models;
using ProductManagement.Core.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ProductManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        public CategoriesController(IUnitOfWork unitOfWork, ILogger<string> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<CategoryDto> Get()
        {
            var categories = _unitOfWork.Categories.GetAll();

            return categories.Select(category => new CategoryDto() { Id = category.CatId, Name = category.Name, CategoryMetadata = _unitOfWork.Categories.GetCategoriesMetadata(category.CatId) }).ToList();
        }


        [HttpGet("{id}")]
        public CategoryDto Get(int id)
        {
            var category = _unitOfWork.Categories.GetById(id);
            return new CategoryDto()
            {
                Id = category.CatId,
                Name = category.Name,
                CategoryMetadata = _unitOfWork.Categories.GetCategoriesMetadata(category.CatId)
            };
        }


        [HttpPost]
        public ResponseDto Post([FromBody] CategoryDto newCategoryDto)
        {
            if (!(ModelState.IsValid))
            {
                var response = new ResponseDto
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSucceed = false,
                    Messages = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList()
                };
                return response;
            }

            try
            {
                Category category = new()
                {
                    Name = newCategoryDto.Name,
                };
                _unitOfWork.Categories.Insert(category);
                _unitOfWork.Complete();

                if (newCategoryDto.CategoryMetadata.ToList().Count > 0)
                {
                    var insertedCategoryId = _unitOfWork.Categories.GetCategoryByName(newCategoryDto.Name).CatId;
                    var metadataList = newCategoryDto.CategoryMetadata.Select(metadata => new CategoriesMetadatum() { CatId = insertedCategoryId, MetadataText = metadata.Text }).ToList();
                    _unitOfWork.CategoriesMetadata.InsertRange(metadataList);
                    _unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new ResponseDto() { StatusCode = (int)HttpStatusCode.Created, IsSucceed = true, Messages = new List<string>() { "Category Created Successfully" } };
        }


        [HttpPut("{id}")]
        public ResponseDto Put(int id, [FromBody] CategoryDto updatedCategoryDto)
        {
            if (!(ModelState.IsValid))
            {
                var response = new ResponseDto
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSucceed = false,
                    Messages = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList()
                };
                return response;
            }

            try
            {
                Category category = new()
                {
                    Name = updatedCategoryDto.Name,
                };
                _unitOfWork.Categories.Update(category);
                _unitOfWork.Complete();

                if (updatedCategoryDto.CategoryMetadata.ToList().Count > 0)
                {
                    var oldMetadataList = _unitOfWork.Categories.GetCategoriesMetadata(id).ToList();
                    var metadataList = updatedCategoryDto.CategoryMetadata.Select(metadata => new CategoriesMetadatum() { CatId = id, MetadataText = metadata.Text }).ToList();

                    var count = oldMetadataList.Count > metadataList.Count ? metadataList.Count : oldMetadataList.Count;
                    for (var i = 0; i < count; i++)
                    {
                        if (oldMetadataList[i].Text != metadataList[i].MetadataText)
                        {
                            _unitOfWork.CategoriesMetadata.Update(metadataList[i]);
                        }

                        oldMetadataList.Remove(oldMetadataList[i]);
                        metadataList.Remove(metadataList[i]);
                    }

                    if (oldMetadataList.Count > 0)
                    {
                        foreach (var item in oldMetadataList)
                        {
                            _unitOfWork.CategoriesMetadata.Delete(item.Id);
                        }
                    }

                    if (metadataList.Count > 0)
                    {
                        foreach (var item in metadataList)
                        {
                            _unitOfWork.CategoriesMetadata.Insert(item);
                        }
                    }


                    _unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new ResponseDto() { StatusCode = (int)HttpStatusCode.Created, IsSucceed = true, Messages = new List<string>() { "Category Updated Successfully" } };
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                _unitOfWork.Categories.Delete(id);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new ResponseDto() { StatusCode = (int)HttpStatusCode.OK, IsSucceed = true, Messages = new List<string>() { "Category Deleted Successfuly" } };
        }
    }
}
