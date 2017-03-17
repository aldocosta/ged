using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IIUD<T>
    {
        int Save(T t);
        bool Delete(T t);
        bool Update(T t);
        IList<T> RetornarLista();
        IList<T> RetornarLista(T t);

        T RetornarEntidade(int id);
    }
}
