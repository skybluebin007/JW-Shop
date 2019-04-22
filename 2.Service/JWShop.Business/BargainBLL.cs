﻿using System;
        /// 删除一条数据
        /// </summary>
        {
            dal.Delete(id);
        }

        /// <summary>
        /// 读取正在进行且有效的活动
        /// </summary>
        /// <returns></returns>
        {
            return dal.View_ReadBargain();
        }
        /// <summary>
        /// 关闭活动事务：关闭活动，将未支付成功的砍价全部置为“砍价失败”，原“活动已取消，砍价失败”
        /// </summary>
        /// <param name="id"></param>
        public static void ChangeBargainStatus(int id,int status)
        {
            if (status == (int)Bargain_Status.ShutDown || status == (int)Bargain_Status.End)
            {
                dal.ChangeBargainStatus(id, status);
            }
        }
    }