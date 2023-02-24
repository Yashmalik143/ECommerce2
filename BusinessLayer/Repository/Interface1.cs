using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repository;

public interface Interface1
{
    public string AddImage(string name, string url,int ProductID);

    public string GetPath(int id);

}
