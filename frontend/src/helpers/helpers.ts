import { checkoutItemStructure, productType, RelatedWordType, trackedItemsType } from "@/types/types";
import { Session } from "next-auth";
import { backendHost } from "../../constants/consts";

async function fetchData<T>(endpoint: string, requestData?: any): Promise<T> {
  const response = await fetch(`${backendHost}${endpoint}`, {
    headers: {
      "Content-Type": "application/json"
    },
    "cache":"no-store",
    body: JSON.stringify(requestData),
  }
  );

  return await response.json()
}

export const fetchProducts = async(queryParamsStr: string) => await fetchData<productType[]>(`Products?${queryParamsStr}`)

export const fetchRelatedWords = async(queryParamsStr: string) => await fetchData<RelatedWordType[]>(`Words?${queryParamsStr}`)

export async function checkout(
  itemsForCheckout: checkoutItemStructure[],
  session: Session | null,
  checkUser: () => void
): Promise<string> {
  try {
    if (itemsForCheckout.length === 0) throw "Nothing to checkout"

    checkUser()

    const response = await fetch(`${backendHost}Checkout`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        checkoutProducts: itemsForCheckout,
        email: session?.user?.email as string
      }),
    });

    const { url }: { url: string } = await response.json();

    if (url) return url
    else throw "no url"
  } catch (error) {
    console.error(error)
    throw error
  }
}


export async function fetchPurchasedItems(orderId: string, email: string): Promise<trackedItemsType> {
  try {
    const items = await fetchData(`OrderStatuses?orderId=${orderId}&email=${email}`) as trackedItemsType

    return items

  } catch (error) {
    throw error
  }
}
