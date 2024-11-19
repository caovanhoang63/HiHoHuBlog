﻿using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Service.Interface;

public interface ISearchTagService
{
    Task<Result<IEnumerable<TagSearchDoc>?, Err>> SearchTags(string arg, Paging paging);
}