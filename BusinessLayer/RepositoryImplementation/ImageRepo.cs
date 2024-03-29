﻿using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RepositoryImplementation;

public class ImageRepo : Interface1
{
    private readonly EcDbContext _db;

    public ImageRepo(EcDbContext db)
    {
        _db = db;
    }

  
    public string AddImage(string name, string url,int ProductID)
    {
        Image image = new Image();
        image.name = name;
        image.url = url;
        image.ProductId = ProductID;
        _db.Images.Add(image);
        _db.SaveChanges();
        var str = $"{name} image";
        return str;
    }

    public async Task< string> GetPath(int Id)
    {
        try
        {
            var ab = await _db.Images.FirstOrDefaultAsync(x => x.ProductId == Id);
            Console.WriteLine(ab);
            return ab.url;
        }
        catch(Exception ex) { return ex.ToString(); }
        
        }
}
