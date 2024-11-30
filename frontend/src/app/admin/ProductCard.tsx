// app/admin/ProductCard.tsx
'use client'

import { useState } from 'react'
import { updateProduct, deleteProduct } from './actions'
import { productType } from '@/types/types'

export default function ProductCard({ product }: { product: productType }) {
    const [isEditing, setIsEditing] = useState(false)
    const [isDeleting, setIsDeleting] = useState(false)

    return (
        <div className="border rounded-lg overflow-hidden shadow-sm">
            <h3>{product.id}</h3>
            <img
                src={product.imageSrc}
                alt={product.itemName}
                className="w-full h-48 object-cover"
            />
            {!isEditing ? (
                <div className="p-4">
                    <h3 className="font-bold text-lg mb-2">{product.itemName}</h3>
                    <p className="text-gray-600 mb-2">{product.description}</p>
                    <p className="text-xl font-bold">${product.price.toFixed(2)}</p>
                    <div className="mt-4 flex gap-2">
                        <button
                            onClick={() => setIsEditing(true)}
                            className="px-3 py-1 bg-blue-500 text-white rounded"
                        >
                            Edit
                        </button>
                        <button
                            onClick={() => setIsDeleting(true)}
                            className="px-3 py-1 bg-red-500 text-white rounded"
                        >
                            Delete
                        </button>
                    </div>
                </div>
            ) : (
                <form 
                    action={async (formData) => {
                        await updateProduct(product.id, formData)
                        setIsEditing(false)
                    }}
                    className="p-4"
                >
                    <input
                        type="file"
                        name="file"
                        accept="image/jpeg,image/png,image/webp"
                        className="mb-2"
                    />
                    <input
                        type="text"
                        name="itemName"
                        defaultValue={product.itemName}
                        className="w-full p-2 border rounded mb-2"
                        required
                    />
                    <textarea
                        name="description"
                        defaultValue={product.description}
                        className="w-full p-2 border rounded mb-2"
                        required
                    />
                    <input
                        type="number"
                        name="price"
                        step="0.01"
                        defaultValue={product.price}
                        className="w-full p-2 border rounded mb-2"
                        required
                    />
                    <div className="flex gap-2">
                        <button
                            type="submit"
                            className="px-3 py-1 bg-green-500 text-white rounded"
                        >
                            Save
                        </button>
                        <button
                            type="button"
                            onClick={() => setIsEditing(false)}
                            className="px-3 py-1 bg-gray-500 text-white rounded"
                        >
                            Cancel
                        </button>
                    </div>
                </form>
            )}
            
            {isDeleting && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
                    <div className="bg-white p-6 rounded-lg">
                        <h3 className="text-lg font-bold mb-4">Delete Product?</h3>
                        <p>Are you sure you want to delete {product.itemName}?</p>
                        <div className="mt-4 flex gap-2">
                            <button
                                onClick={async () => {
                                    await deleteProduct(product.id)
                                    setIsDeleting(false)
                                }}
                                className="px-3 py-1 bg-red-500 text-white rounded"
                            >
                                Delete
                            </button>
                            <button
                                onClick={() => setIsDeleting(false)}
                                className="px-3 py-1 bg-gray-500 text-white rounded"
                            >
                                Cancel
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    )
}