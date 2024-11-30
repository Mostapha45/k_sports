"use client"

import { purchasedItemsFormat } from "@/types/types";
import { useSession } from "next-auth/react";
import { useCallback, useRef, useState } from "react";

export default function TrackMyOrderPage() {
  const [purchasedItems, setPurchasedItems] = useState<purchasedItemsFormat[]>([]);
  const orderIdInput = useRef<HTMLInputElement>(null);
  const [deliverStatus, setDeliverStatus] = useState<string>()
  const [error, setError] = useState<string | undefined>()

  const { data: session, status } = useSession()

  const getPurchasedItems = useCallback(async (e: React.FormEvent<HTMLFormElement>) => {

    setError(undefined)
    e.preventDefault();

    if (status !== "authenticated") {
      setError("Please login to track your order")
      return
    }

    const email = session?.user?.email;
    if (!email) {
      setError("Email not found")
      return
    }

    const orderIdElement = orderIdInput.current

    if (!orderIdElement) {
      setError("Order ID input not found")
      return
    }

    try {

      const { fetchPurchasedItems } = await import("@/utils/utils")

      const { deliveryDate, products } = await fetchPurchasedItems(orderIdElement.value, session?.user?.email as string);

      if (deliveryDate && products.length > 0) {
        const deliverDate = new Date(deliveryDate)

        const today = new Date();
        const deliverDateString = deliverDate.toLocaleDateString('en-US', { month: 'long', day: 'numeric', year: 'numeric' });

        setDeliverStatus(() => {
          if (today < deliverDate) return `Products will be delivered at ${deliverDateString}`
          if (today === deliverDate) return `Products will be delivered today`
          return `Products should have been delivered. If not, please contact KSports`
        }
        )

        setPurchasedItems(products)
      } else {
        setError("No products found for this order")
      }
    } catch (err) {
      setError((err as Error).message || "An error occurred")
    }
  }, [session, status])

  return (
    <>
      <div className="my-[6vh] mx-[4vw]">
        <title>Track My Order</title>
        <form onSubmit={getPurchasedItems}>
          <p className="text-center my-[5vh] text-3xl">Track Order Status</p>
          <input
            ref={orderIdInput}
            name="orderId"
            type="text"
            className="
            border-2
            border-gray-500
            w-full
            text-center
            rounded-md
            placeholder-gray-700
            mb-5
            py-3"
            placeholder="Enter your Order Number here"
          />
          <button
            type="submit"
            className="w-full p-[2vh] bg-blue-600 text-white font-bold text-xl"
          >
            Track
          </button>
        </form>

        {purchasedItems.length > 0 ? (
          <div className="mt-14">

            <h3>{deliverStatus}</h3>
            {
              purchasedItems.map((item: purchasedItemsFormat, index) => {
                return (
                  <div
                    key={index}
                    className="flex justify-evenly items-center rounded-md mx-0 my-[2vh] py-[3vh] border border-black"
                  >
                    <p className="text-xl">{item.name}</p>
                    <p className="text-xl">
                      Price: ${item.price / 100}
                    </p>
                    <p className="text-xl">Quantity: {item.quantity}</p>
                  </div>
                );
              })
            }
          </div>
        ) :
          error ? <h3 className="text-red-500 font-semibold text-lg">{error}</h3> :
            <h3>No data</h3>
        }
      </div>
    </>
  );
}