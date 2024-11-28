import { PRICE_SORT } from "../../constants/consts"

export interface SearchParams {
    sort?: string
    limit?: string
    contains?: string
    keywords?: string
}

export type NavLinkProp = {
    title: string,
    link: string
}

export type SortType = (typeof PRICE_SORT)[number];

export type RelatedWordType = {
    word: string,
    productCount: number
}

export type productType = {
    id: number,
    itemName: string;
    authorLink?: string;
    authorName?: string;
    imageCredit?: string;
    imageSrc: string;
    price: number;
    quantity?: number;
    description: string
};

export type checkoutItemStructure = {
    PriceData: {
        Currency: string;
        ProductData: { Name: string };
        UnitAmount: number;
    },
    Quantity: number;
};

export type trackedItemsType = {
    deliveryDate: string,
    products: purchasedItemsFormat[]
}

export type purchasedItemsFormat = {
    price: number,
    name: string,
    quantity: number
};
