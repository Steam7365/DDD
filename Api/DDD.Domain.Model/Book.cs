using System.ComponentModel.DataAnnotations;

namespace DDD.Domain.Model
{
    public class Book
    {
        public int Id { get;set;}

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get;set;} 

        /// <summary>
        /// 书名
        /// </summary>

        public string Name { get; set; } 

        /// <summary>
        /// 描述
        /// </summary>

        public string Description { get; set; }
        /// <summary>
        /// 作者
        /// </summary>

        public string Author { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public string PubTime { get; set; }
    }
}
