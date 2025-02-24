using System.Reflection;
using GYM_BE.Core.Dto;

namespace CORE.AutoMapper;

public static class CoreMapper<TDTO, TEntity> where TDTO : class where TEntity : class
{
    public static TEntity? DtoToEntity(TDTO dto, TEntity entity, bool? patchMode = true)
    {
        TDTO dto2 = dto;
        TEntity entity2 = entity;
        Type typeFromHandle = typeof(TDTO);
        List<PropertyInfo> dtoProperties = typeFromHandle.GetProperties().ToList();
        PropertyInfo propertyInfo = dtoProperties.SingleOrDefault((PropertyInfo x) => x.Name == "ActualFormDeclaredFields");
        List<string> actualFormDeclaredFields = new List<string>();
        if (propertyInfo != null)
        {
            actualFormDeclaredFields = (List<string>)propertyInfo.GetValue(dto2);
            if (actualFormDeclaredFields == null)
            {
                actualFormDeclaredFields = new List<string>();
            }
        }

        List<PropertyInfo> list = (from pi in typeof(TEntity).GetProperties()
                                   where !pi.GetAccessors()[0].IsVirtual
                                   select pi).ToList();
        List<string> alwaysIgnoredColumns = new List<string> { "Id", "CreatedDate", "CreatedBy", "UpdatedDate", "UpdatedBy" };
        list.ForEach(delegate (PropertyInfo piEntity)
        {
            string name = piEntity.Name;
            string dtoPropertyName = name.SnakeToCamelCase().CamelToPascalCase();
            PropertyInfo propertyInfo2 = dtoProperties.Where((PropertyInfo p) => p.Name == dtoPropertyName).FirstOrDefault();
            if (propertyInfo2 != null)
            {
                object value = propertyInfo2.GetValue(dto2);
                if (value != null)
                {
                    piEntity.SetValue(entity2, value);
                }
                else if (patchMode == true)
                {
                    bool num = actualFormDeclaredFields.Select((string x) => x.CamelToPascalCase()).Any((string x) => x == dtoPropertyName);
                    bool flag = alwaysIgnoredColumns.IndexOf(dtoPropertyName) == -1;
                    if (num && flag)
                    {
                        piEntity.SetValue(entity2, value);
                    }
                }
                else
                {
                    piEntity.SetValue(entity2, value);
                }
            }
        });
        return entity2;
    }

    public static TDTO? EntityToDto(TEntity entity, TDTO dto)
    {
        TEntity entity2 = entity;
        TDTO dto2 = dto;
        Type entityType = typeof(TEntity);
        List<PropertyInfo> entityProperties = entityType.GetProperties().ToList();
        Type dtoType = typeof(TDTO);
        (from x in dtoType.GetProperties()
         where entityProperties.Any((PropertyInfo ep) => ep.Name.SnakeToCamelCase().CamelToPascalCase() == x.Name)
         select x).ToList().ForEach(delegate (PropertyInfo piDto)
         {
             string name = piDto.Name;
             string entityPropertyName = name.CamelOrPascalToSnackCase();
             PropertyInfo propertyInfo = entityProperties.Where((PropertyInfo p) => p.Name == entityPropertyName).FirstOrDefault();
             if (propertyInfo != null)
             {
                 object value = propertyInfo.GetValue(entity2);
                 piDto.SetValue(dto2, value);
                 return;
             }

             throw new Exception($"Property {name} of entity {dtoType.Name} has no corresponding property in {entityType.Name} class");
         });
        return dto2;
    }

    public static List<TEntity>? ListDtoToListEntity(List<TDTO> dtos)
    {
        List<TEntity> list = new List<TEntity>();
        for (int i = 0; i < dtos.Count; i++)
        {
            TDTO dto = dtos[i];
            TEntity entity = Activator.CreateInstance<TEntity>();
            TEntity val = DtoToEntity(dto, entity, true);
            if (val != null)
            {
                list.Add(val);
                continue;
            }

            return null;
        }

        return list;
    }

    public static List<TDTO>? ListEntityToListDTO(List<TEntity> entities)
    {
        List<TDTO> list = new List<TDTO>();
        for (int i = 0; i < entities.Count; i++)
        {
            TEntity entity = entities[i];
            TDTO dto = Activator.CreateInstance<TDTO>();
            TDTO val = EntityToDto(entity, dto);
            if (val != null)
            {
                list.Add(val);
                continue;
            }

            return null;
        }

        return list;
    }
}