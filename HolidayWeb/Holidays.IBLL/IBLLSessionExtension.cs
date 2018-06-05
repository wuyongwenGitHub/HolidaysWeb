
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.IBLL
{
	public partial interface IBLLSession
    {
		IAdminUserBLL IAdminUserBLL{get;set;}
		ICarInfoBLL ICarInfoBLL{get;set;}
		IHomeDataBLL IHomeDataBLL{get;set;}
		IHouseCommentBLL IHouseCommentBLL{get;set;}
		IHouseConfigureBLL IHouseConfigureBLL{get;set;}
		IHouseEvaluateBLL IHouseEvaluateBLL{get;set;}
		IHouseEvaluateViewBLL IHouseEvaluateViewBLL{get;set;}
		IHouseImgBLL IHouseImgBLL{get;set;}
		IHouseInfoBLL IHouseInfoBLL{get;set;}
		IHouseInfoViewBLL IHouseInfoViewBLL{get;set;}
		IOrderInfoBLL IOrderInfoBLL{get;set;}
		IOrderInfoViewBLL IOrderInfoViewBLL{get;set;}
		IOrderItemBLL IOrderItemBLL{get;set;}
		IProductCategoryBLL IProductCategoryBLL{get;set;}
		IProductInfoBLL IProductInfoBLL{get;set;}
		IRegionBLL IRegionBLL{get;set;}
		IShopCategoryBLL IShopCategoryBLL{get;set;}
		IShopInfoBLL IShopInfoBLL{get;set;}
        IShopToDaySetBll IShopToDaySetBll { get; set; }

        IShopInfoCertificateBLL IShopInfoCertificateBLL{get;set;}
		IShopInfoCertificateViewBLL IShopInfoCertificateViewBLL{get;set;}
		ISpotInfoBLL ISpotInfoBLL{get;set;}
		ISysLogBLL ISysLogBLL{get;set;}
		ISystemConfigBLL ISystemConfigBLL{get;set;}
		ITenantInfoBLL ITenantInfoBLL{get;set;}
		IUserAccountBLL IUserAccountBLL{get;set;}
		IUserFavoriteBLL IUserFavoriteBLL{get;set;}
		IUserFavoriteViewBLL IUserFavoriteViewBLL{get;set;}
		IUserInfoBLL IUserInfoBLL{get;set;}
		IUserInfoCertificateBLL IUserInfoCertificateBLL{get;set;}
		IUserInfoCertificateViewBLL IUserInfoCertificateViewBLL{get;set;}
		IUserInfoExtBLL IUserInfoExtBLL{get;set;}
		IUserInfoExtViewBLL IUserInfoExtViewBLL{get;set;}
		IUserInfoViewBLL IUserInfoViewBLL{get;set;}
		IUserOrderRecordBLL IUserOrderRecordBLL{get;set;}
        IUserBLL IUserBLL { get; set; }
        IPermissionBLL IPermissionBLL { get; set; }
        IRoleBLL IRoleBLL { get; set; }
        IFunctionBLL IFunctionBLL { get; set; }
        IFuncPermissionBLL IFuncPermissionBLL { get; set; }
        IUserRoleBLL IUserRoleBLL { get; set; }
    }
}