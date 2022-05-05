using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Dynamic.Core;

public class Search<T> : ISearch<T> 
{
    public IEnumerable<T>? ApplySearch(IEnumerable<T>? entities, string? searchString)
    {
        
        if (entities==null || !entities.Any())
            return entities;
        if (string.IsNullOrWhiteSpace(searchString))
        {
            return entities;
        }

        var searchedEntities = entities.ToList();
        //Entities cycle
        foreach (var entity in entities)
        {
            var flag=false;
            //Properties cycle
            if (entity==null)
                continue;

            foreach (var property in entity.GetType().GetProperties())
            {

                if ( property.PropertyType == typeof(string))
                {
                    if (property.GetValue(entity)!=null && (property.GetValue(entity)?.ToString() ?? "").ToLower().Contains(searchString.ToLower()))
                    {
                        flag=true;
                        break;
                    }
                }
                
            }
            //If the entity dont contains the search string, delete it to the result
            if (!flag)
            {
                searchedEntities.Remove(entity);            
            }
        }
        return searchedEntities;
    }
}







