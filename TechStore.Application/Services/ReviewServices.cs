using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos.ReviewDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public class ReviewServices : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _UserRepository;

        public ReviewServices(IReviewRepository reviewRepository, IMapper mapper,IUserRepository userRepository)
        {

            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _UserRepository= userRepository;

        }
        public async Task<ResultView<CreateOrUpdateReviewDto>> CreateReview(CreateOrUpdateReviewDto Review)
        {

            var review = _mapper.Map<Review>(Review);
            var NewReview = await _reviewRepository.CreateAsync(review);
            await _reviewRepository.SaveChangesAsync();
            var ReviewDto = _mapper.Map<CreateOrUpdateReviewDto>(NewReview);
            return new ResultView<CreateOrUpdateReviewDto>() { Entity = ReviewDto, IsSuccess = true, Message = "Created Successfully" };


        }

        public async Task<ResultDataList<GetAllReviewDto>> GetAllPaginationReview(int items, int pagenumber)
        {
            var AllData = await _reviewRepository.GetAllAsync();
            var Reviews =( await _reviewRepository.GetAllAsync()).Skip(items * pagenumber - 1).Take(items).
                  Select(p => new GetAllReviewDto
                  {
                      Id = p.Id,
                      // TechUserId = p.TechUserId,
                      Comment = p.Comment,
                      //  ProductId=p.ProductId,
                      Rating = p.Rating,
                      ReviewDate = (DateTime)p.ReviewDate,
                  }).ToList();


            ResultDataList<GetAllReviewDto> resultDataList = new ResultDataList<GetAllReviewDto>();
            resultDataList.Entities = Reviews;
            resultDataList.Count = AllData.Count();
            return resultDataList;
        }

        public async Task<CreateOrUpdateReviewDto> GetOneReview(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            var reviewDto = _mapper.Map<CreateOrUpdateReviewDto>(review);
            return reviewDto;
        }

        public async Task<ResultView<CreateOrUpdateReviewDto>> HardDeleteReview(CreateOrUpdateReviewDto Review)
        {
            try
            {

                var review = _mapper.Map<Review>(Review);
                var oldreview = await _reviewRepository.DeleteAsync(review);
                await _reviewRepository.SaveChangesAsync();
                var ReviewDto = _mapper.Map<CreateOrUpdateReviewDto>(oldreview);
                return new ResultView<CreateOrUpdateReviewDto> { Entity = ReviewDto, IsSuccess = true, Message = "Deleted Successfully" };

            }
            catch (Exception ex)
            {
                return new ResultView<CreateOrUpdateReviewDto> { Entity = null, IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<ResultView<CreateOrUpdateReviewDto>> SoftDeleteReview(CreateOrUpdateReviewDto Review)
        {

            try
            {

                var review = _mapper.Map<Review>(Review);
                var oldreview = (await _reviewRepository.GetAllAsync()).FirstOrDefault(p => p.Id == Review.Id);
                oldreview.IsDeleted = true;
                await _reviewRepository.SaveChangesAsync();
                var reviewDto = _mapper.Map<CreateOrUpdateReviewDto>(oldreview);
                return new ResultView<CreateOrUpdateReviewDto> { Entity = reviewDto, IsSuccess = true, Message = "Deleted Successfully" };

            }
            catch (Exception ex)
            {
                return new ResultView<CreateOrUpdateReviewDto> { Entity = Review, IsSuccess = false, Message = ex.Message };
            }


        }

        public async Task<ResultView<CreateOrUpdateReviewDto>> UpdateReview(CreateOrUpdateReviewDto Review)
        {

            var review = _mapper.Map<Review>(Review);
            var Newreview = await _reviewRepository.UpdateAsync(review);
            await _reviewRepository.SaveChangesAsync();
            var reviewDto = _mapper.Map<CreateOrUpdateReviewDto>(Newreview);
            return new ResultView<CreateOrUpdateReviewDto> { Entity = reviewDto, IsSuccess = true, Message = "Updated Successfully" };


        }

        public async Task<ResultDataList<GetAllReviewDto>> GetAllReviewByProduct(int items, int pageNumber, int productId)
        {

            var reviewsWithUsers = (await _reviewRepository.GetAllAsync()).Where(review => review.ProductId == productId && review.IsDeleted==false)
                .OrderByDescending(review => review.Id)
                .Skip(items * (pageNumber - 1))
                .Take(items)
                .Join(
                  await _UserRepository.GetAllAsync(),
                    review => review.UserId,
                    user => user.Id,
                    (review, user) => new GetAllReviewDto
                    {
                        UserName = user.FirstName+user.LastName,
                        UserId = user.Id,
                        ProductId= review.ProductId,
                        Id = review.Id,
                        Comment = review.Comment,
                      //  User = user,
                        Rating = review.Rating,
                        ReviewDate = (DateTime)review.ReviewDate
                    }
                )
                .ToList();


            var totalCount = (await _reviewRepository.GetAllAsync())
                .Where(review => review.ProductId == productId && review.IsDeleted == false);


            ResultDataList<GetAllReviewDto> resultDataList = new ResultDataList<GetAllReviewDto>();
            resultDataList.Entities = reviewsWithUsers;
            resultDataList.Count = totalCount.Count();
            return resultDataList;
        }

    }
}
