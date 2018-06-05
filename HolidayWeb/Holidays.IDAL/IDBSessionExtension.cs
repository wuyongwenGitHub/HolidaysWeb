
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.IDAL
{
	public partial interface IDBSession
    {
		IAdminUserDAL IAdminUserDAL{get;set;}
		ICarInfoDAL ICarInfoDAL{get;set;}
		IHomeDataDAL IHomeDataDAL{get;set;}
		IHouseCommentDAL IHouseCommentDAL{get;set;}
		IHouseConfigureDAL IHouseConfigureDAL{get;set;}
		IHouseEvaluateDAL IHouseEvaluateDAL{get;set;}
		IHouseEvaluateViewDAL IHouseEvaluateViewDAL{get;set;}
		IHouseImgDAL IHouseImgDAL{get;set;}
		IHouseInfoDAL IHouseInfoDAL{get;set;}
		IHouseInfoViewDAL IHouseInfoViewDAL{get;set;}
		IOrderInfoDAL IOrderInfoDAL{get;set;}
		IOrderInfoViewDAL IOrderInfoViewDAL{get;set;}
		IOrderItemDAL IOrderItemDAL{get;set;}
		IProductCategoryDAL IProductCategoryDAL{get;set;}
		IProductInfoDAL IProductInfoDAL{get;set;}
		IRegionDAL IRegionDAL{get;set;}
		IShopCategoryDAL IShopCategoryDAL{get;set;}
        IShopToDayPriceSetDAL IShopToDayPriceSetDAL { get; set; }
        IShopInfoDAL IShopInfoDAL{get;set;}
		IShopInfoCertificateDAL IShopInfoCertificateDAL{get;set;}
		IShopInfoCertificateViewDAL IShopInfoCertificateViewDAL{get;set;}
		ISpotInfoDAL ISpotInfoDAL{get;set;}
		ISysLogDAL ISysLogDAL{get;set;}
		ISystemConfigDAL ISystemConfigDAL{get;set;}
		ITenantInfoDAL ITenantInfoDAL{get;set;}
		IUserAccountDAL IUserAccountDAL{get;set;}
		IUserFavoriteDAL IUserFavoriteDAL{get;set;}
		IUserFavoriteViewDAL IUserFavoriteViewDAL{get;set;}
		IUserInfoDAL IUserInfoDAL{get;set;}
		IUserInfoCertificateDAL IUserInfoCertificateDAL{get;set;}
		IUserInfoCertificateViewDAL IUserInfoCertificateViewDAL{get;set;}
		IUserInfoExtDAL IUserInfoExtDAL{get;set;}
		IUserInfoExtViewDAL IUserInfoExtViewDAL{get;set;}
		IUserInfoViewDAL IUserInfoViewDAL{get;set;}
		IUserOrderRecordDAL IUserOrderRecordDAL{get;set;}
        IUserDAL IUserDAL { get; set; }
        IPermissionDAL IPermissionDAL { get; set; }
        IRoleDAL IRoleDAL { get; set; }
      IFunctionDAL IFunctionDAL { get; set; }
        IFuncPermissionDAL IFuncPermissionDAL { get; set; }
        IUserRoleDAL IUserRoleDAL { get; set; }
        
    }
}