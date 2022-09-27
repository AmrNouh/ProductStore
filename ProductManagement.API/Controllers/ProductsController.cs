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
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public ProductsController(IUnitOfWork unitOfWork, ILogger<string> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        [HttpGet]
        public IEnumerable<ProductDto> Get()
        {
            var products = _unitOfWork.Products.GetAll();

            return products.Select(product => new ProductDto()
            {
                Id = product.ProdId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CreatedAt = product.CreatedAt,
                CategoryName = _unitOfWork.Categories.GetById(product.CategoryId).Name,
                IsArchived = product.IsArchived,
                ProductMetadata = _unitOfWork.Products.GetProductMetadata(product.ProdId)
            })
                .ToList();

        }


        [HttpGet("{id}")]
        public ProductDto Get(int id)
        {
            var product = _unitOfWork.Products.GetById(id);
            return new ProductDto()
            {
                Id = product.ProdId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CreatedAt = product.CreatedAt,
                CategoryName = _unitOfWork.Categories.GetById(product.CategoryId).Name,
                IsArchived = product.IsArchived,
                ProductMetadata = _unitOfWork.Products.GetProductMetadata(product.ProdId)
            };
        }


        [HttpPost]
        public ResponseDto Post([FromBody] ProductDto newProduct)
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
                Product product = new()
                {
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    Price = newProduct.Price,
                    CreatedAt = newProduct.CreatedAt,
                    CategoryId = _unitOfWork.Categories.GetCategoryByName(newProduct.CategoryName).CatId,
                    IsArchived = newProduct.IsArchived
                };
                _unitOfWork.Products.Insert(product);
                _unitOfWork.Complete();

                if (newProduct.ProductMetadata.ToList().Count > 0)
                {
                    var insertedProductId = _unitOfWork.Products.GetProductByName(newProduct.Name).ProdId;
                    var metaList = newProduct.ProductMetadata.Select(metadata => new ProductsMetadatum() { ProdId = insertedProductId, MetadataId = metadata.Id }).ToList();
                    _unitOfWork.ProductsMetadata.InsertRange(metaList);
                    _unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new ResponseDto() { StatusCode = (int)HttpStatusCode.Created, IsSucceed = true, Messages = new List<string>() { "Product Created Successfully" } };
        }


        [HttpPut("{id}")]
        public ResponseDto Put(int id, [FromBody] ProductDto updatedProduct)
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
                Product product = new()
                {
                    Name = updatedProduct.Name,
                    Description = updatedProduct.Description,
                    Price = updatedProduct.Price,
                    CreatedAt = updatedProduct.CreatedAt,
                    CategoryId = _unitOfWork.Categories.GetCategoryByName(updatedProduct.CategoryName).CatId,
                    IsArchived = updatedProduct.IsArchived
                };
                _unitOfWork.Products.Update(product);

                if (updatedProduct.ProductMetadata.ToList().Count > 0)
                {
                    var oldMetadataList = _unitOfWork.Products.GetProductMetadata(id).ToList();
                    var metadataList = updatedProduct.ProductMetadata.Select(metadata => new ProductsMetadatum() { ProdId = id, MetadataId = metadata.Id }).ToList();
                    var count = oldMetadataList.Count > metadataList.Count ? metadataList.Count : oldMetadataList.Count;

                    for (var i = 0; i < count; i++)
                    {
                        if (oldMetadataList[i].Id != metadataList[i].MetadataId)
                        {
                            _unitOfWork.ProductsMetadata.Update(metadataList[i]);
                        }

                        oldMetadataList.Remove(oldMetadataList[i]);
                        metadataList.Remove(metadataList[i]);
                        count--;
                        i = -1;
                    }

                    if (oldMetadataList.Count > 0)
                    {
                        foreach (var item in oldMetadataList)
                        {
                            _unitOfWork.ProductsMetadata.Delete(item.Id);
                        }
                    }

                    if (metadataList.Count > 0)
                    {
                        foreach (var item in metadataList)
                        {
                            _unitOfWork.ProductsMetadata.Insert(item);
                        }
                    }


                    _unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new ResponseDto() { StatusCode = (int)HttpStatusCode.NoContent, IsSucceed = true, Messages = new List<string>() { "Product Updated Successfully" } };
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            try
            {
                _unitOfWork.Products.Delete(id);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        [HttpGet("search/{productName}")]
        public List<ProductDto> SearchByName(string productName)
        {
            List<ProductDto> productDtoList = new();
            var products = _unitOfWork.Products.GetProductsByName(productName);

            if (products.Count == 0)
                return productDtoList;

            productDtoList.AddRange(products.Select(product => new ProductDto()
            {
                Id = product.ProdId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CreatedAt = product.CreatedAt,
                CategoryName = _unitOfWork.Categories.GetById(product.CategoryId).Name,
                IsArchived = product.IsArchived,
                ProductMetadata = _unitOfWork.Products.GetProductMetadata(product.ProdId)
            }));
            return productDtoList;
        }

        [HttpGet("search/{createdDate}")]
        public List<ProductDto> SearchByDate(DateTime createdDate)
        {
            List<ProductDto> productDtoList = new();
            var products = _unitOfWork.Products.GetProductsByDate(createdDate);

            if (products.Count == 0)
                return productDtoList;

            productDtoList.AddRange(products.Select(product => new ProductDto()
            {
                Id = product.ProdId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CreatedAt = product.CreatedAt,
                CategoryName = _unitOfWork.Categories.GetById(product.CategoryId).Name,
                IsArchived = product.IsArchived,
                ProductMetadata = _unitOfWork.Products.GetProductMetadata(product.ProdId)
            }));
            return productDtoList;
        }

        [HttpGet("search    /{metadataText}")]
        public List<ProductDto> SearchByMetaDataText(string metadataText)
        {
            List<ProductDto> productDtoList = new();
            var products = _unitOfWork.Products.GetProductsByMetadataText(metadataText);

            if (products.Count == 0)
                return productDtoList;

            productDtoList.AddRange(products.Select(product => new ProductDto()
            {
                Id = product.ProdId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CreatedAt = product.CreatedAt,
                CategoryName = _unitOfWork.Categories.GetById(product.CategoryId).Name,
                IsArchived = product.IsArchived,
                ProductMetadata = _unitOfWork.Products.GetProductMetadata(product.ProdId)
            }));
            return productDtoList;
        }
    }
}
