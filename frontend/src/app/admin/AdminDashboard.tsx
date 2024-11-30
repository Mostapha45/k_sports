import { createProduct } from "./actions";
import { fetchProducts } from "../../utils/utils";
import ProductCard from "./ProductCard";

export default async function AdminDashboard() {

    const products = await fetchProducts(`shouldTakeAll=True`);

    return (
        <div className="p-8">
            <h1 className="text-2xl font-bold mb-6">Admin Dashboard</h1>

            {/* Product Creation Form */}
            <form action={createProduct} className="mb-8 space-y-4 max-w-xl">
                <div>
                    <label className="block mb-2">Product Image</label>
                    <input
                        type="file"
                        name="file"
                        accept="image/jpeg,image/png,image/webp"
                        className="w-full p-2 border rounded"
                        required
                    />
                </div>

                <div>
                    <label className="block mb-2">Product Name</label>
                    <input
                        type="text"
                        name="itemName"
                        className="w-full p-2 border rounded"
                        required
                    />
                </div>

                <div>
                    <label className="block mb-2">Description</label>
                    <textarea
                        name="description"
                        rows={4}
                        className="w-full p-2 border rounded"
                        required
                    />
                </div>

                <div>
                    <label className="block mb-2">Price</label>
                    <input
                        type="number"
                        name="price"
                        step="0.01"
                        min="0"
                        className="w-full p-2 border rounded"
                        required
                    />
                </div>

                <button
                    type="submit"
                    className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
                >
                    Create Product
                </button>
            </form>

            {/* Products List */}
            <h2 className="text-xl font-bold mb-4">Current Products</h2>

            <div className="mt-10 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {products.map(product => <ProductCard key={product.id} product={product} />)}
            </div>
        </div>
    )
}