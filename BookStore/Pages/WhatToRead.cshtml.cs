using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages;

public class WhatToReadModel : PageModel
{
    public readonly IServiceUser _serviceUser;
    public readonly IServiceBook _serviceBook;

    public WhatToReadModel(IServiceBook serviceBook, IServiceUser serviceUser)
    {
        _serviceUser = serviceUser;
        _serviceBook = serviceBook;
    }
    public void OnGet()
    {
    }
}
