﻿using IonosLedWebMvc.Ver2.Dtos;
using Microsoft.EntityFrameworkCore;

namespace IonosLedWebMvc.Ver2.Models
{
    public class PaginatedList<T>
    {
        public List<LedLampDto> LampDtoItems { get; set; } = new List<LedLampDto>();
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; private set; }
        public LedLampDto LampDto { get; set; } = new LedLampDto();
        public bool IsTodayRange = true;
        public bool IsFirstEnter = false;

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
            IsFirstEnter = false;
        }
        public PaginatedList(bool isFirstEnter)
        {
            IsFirstEnter = isFirstEnter;
        }

        public bool HasPreviousPage => (PageIndex > 1);
        public bool HasNextPage => (PageIndex < TotalPages);

        public int FirstItemIndex => (PageIndex - 1) * PageSize + 1;
        public int LastItemIndex => Math.Min(PageIndex * PageSize, TotalItems);

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync(); //total number of items in the source data.      
            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);

        }

    }
}
