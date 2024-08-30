using AutoMapper;
using Domain.Entities;
using Service.ViewModels.Banner;
using Service.ViewModels.Brand;
using Service.ViewModels.Category;
using Service.ViewModels.Color;
using Service.ViewModels.InfoBanner;
using Service.ViewModels.Product;
using Service.ViewModels.ProductColorVms;
using Service.ViewModels.ProductImageVMs;
using Service.ViewModels.ProductTagVMs;
using Service.ViewModels.Setting;
using Service.ViewModels.Slider;
using Service.ViewModels.Tag;

namespace Service.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductCreateVM, Product>();
            CreateMap<Product, ProductDetailVM>();
            CreateMap<ProductDetailVM, ProductUpdateVM>();
            CreateMap<ProductUpdateVM, Product>();
            CreateMap<ProductColorCreateVM, ProductColor>();
            CreateMap<ProductColor, ProductColorListVM>();
            CreateMap<ProductTagCreateVM, ProductTag>();
            CreateMap<ProductImageCreateVM, ProductImage>();

            CreateMap<CategoryUpdateVM, Category>();
            CreateMap<CategoryCreateVM, Category>();
            CreateMap<Category, CategoryVM>();
            CreateMap<Category, CategoryUpdateVM>();
            CreateMap<Category, CategoryListVM>();

            CreateMap<ColorUpdateVM, Color>();
            CreateMap<ColorCreateVM, Color>();
            CreateMap<Color, ColorVM>();
            CreateMap<Color, ColorUpdateVM>();
            CreateMap<Color, ColorListVM>();


            CreateMap<TagUpdateVM, Tag>();
            CreateMap<TagCreateVM, Tag>();
            CreateMap<Tag, TagVM>();
            CreateMap<Tag, TagUpdateVM>();
            CreateMap<Tag, TagListVM>();

            CreateMap<Banner, BannerUpdateVM>();
            CreateMap<Banner, BannerVM>();
            CreateMap<BannerUpdateVM, Banner>();
            CreateMap<Banner, BannerListVM>();
            CreateMap<BannerCreateVM, Banner>();

            CreateMap<Brand, BrandUpdateVM>();
            CreateMap<Brand, BrandVM>();
            CreateMap<BrandUpdateVM, Brand>();
            CreateMap<Brand, BrandListVM>();
            CreateMap<BrandCreateVM, Brand>();



            CreateMap<Slider, SliderUpdateVM>();
            CreateMap<Slider, SliderVM>();
            CreateMap<SliderUpdateVM, Slider>();
            CreateMap<Slider, SliderListVM>();
            CreateMap<SliderCreateVM, Slider>();

            CreateMap<InfoBanner, InfoBannerUpdateVM>();
            CreateMap<InfoBanner, InfoBannerVM>();
            CreateMap<InfoBannerUpdateVM, InfoBanner>();
            CreateMap<InfoBanner, InfoBannerListVM>();
            CreateMap<InfoBannerCreateVM, InfoBanner>();

            CreateMap<Setting, SettingUpdateVM>();
            CreateMap<Setting, SettingVM>();
            CreateMap<SettingUpdateVM, Setting>();
            CreateMap<Setting, SettingListVM>();
            CreateMap<SettingCreateVM, Setting>();
        }
    }
}

