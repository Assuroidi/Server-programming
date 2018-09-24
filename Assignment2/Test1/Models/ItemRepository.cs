// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace Test1.Models
// {
//     public class ItemRepository : IRepository<Item, ModifiedItem>
//     {
//         public List<Item> itemList = new List<Item>();

//         public async Task<Item> Get(Guid id)
//         {
//             foreach(var itemvar in itemList)
//             {
//                 if(itemvar.ItemId  == id)
//                 {
//                     return itemvar;
//                 }
//             }
//             return null;
//         }

//         public async Task<Item[]> GetAll()
//         {
//             return itemList.ToArray();
//         }

//         public async Task<Item> Create(Item item)
//         {
//             itemList.Add(item);
//             return item;
//         }

//         public async Task<Item> Modify(Guid id, ModifiedItem item)
//         {
//             foreach(var itemvar in itemList)
//             {
//                 if (itemvar.ItemId == id)
//                 {
//                     itemvar.Level = item.Level;
//                     return itemvar;
//                 }


//             }
//             return null;
//         }
        
//         public async Task<Item> Delete(Guid id)
//         {
//             foreach(var itemvar in itemList)
//             {
//                 if (itemvar.ItemId == id)
//                 {
//                     itemList.Remove(itemvar);
//                     return itemvar;
//                 }

//             }
//             return null;

//         }


//     }
// }