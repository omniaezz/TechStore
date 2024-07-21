using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos.ReviewDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public interface IReviewService
    {
        Task<ResultView<CreateOrUpdateReviewDto>> CreateReview( CreateOrUpdateReviewDto createOrUpdateReview);
        Task<ResultView<CreateOrUpdateReviewDto>> UpdateReview(CreateOrUpdateReviewDto createOrUpdateReview);
        Task<ResultView<CreateOrUpdateReviewDto>> HardDeleteReview(CreateOrUpdateReviewDto createOrUpdateReview);
        Task<ResultView<CreateOrUpdateReviewDto>> SoftDeleteReview(CreateOrUpdateReviewDto createOrUpdateReview);
        Task<ResultDataList<GetAllReviewDto>> GetAllPaginationReview(int items, int pagenumber );
        Task<ResultDataList<GetAllReviewDto>> GetAllReviewByProduct(int items, int pagenumber, int id);
        Task<CreateOrUpdateReviewDto> GetOneReview(int id);


    }
}
