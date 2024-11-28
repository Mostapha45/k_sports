"use client"

import { useState } from "react";
import ShopOptions from "./Options/ShopOptions";
import DisplayOrientationSelections from "./Options/DisplayOrientationSelections";
import AddedItemNotifications from "../AddedItemNotifications";
import { productType } from "@/types/types";
import DisplayItems from "./Display/DisplayItems";

export default function MainContent({ products, searchParams }: { searchParams: any, products: productType[] }) {

    const [isDisplayGrid, setIsDisplayGrid] = useState(false)

    return <>
        <div className="flex justify-between items-center">
            <DisplayOrientationSelections isDisplayGrid={isDisplayGrid} setIsDisplayGrid={setIsDisplayGrid} />
            <AddedItemNotifications />
            <ShopOptions searchParams={searchParams}/>
        </div>
        <DisplayItems isDisplayGrid={isDisplayGrid} products={products} />
    </>
}