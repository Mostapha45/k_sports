'use server'

import { BlobServiceClient } from '@azure/storage-blob'
import { PrismaClient } from '@prisma/client'
import { revalidatePath } from 'next/cache'

const prisma = new PrismaClient()

export async function createProduct(formData: FormData) {
    // Upload image first
    const imageResult = await uploadProductImage(formData)

    // Create product with image URL
    await prisma.products.create({
        data: {
            ItemName: formData.get('itemName') as string,
            Description: formData.get('description') as string,
            Price: parseFloat(formData.get('price') as string),
            ImageSrc: imageResult.url,
        }
    })

    revalidatePath('/admin')
    revalidatePath('/')
}

async function uploadToAzure(file: File) {
    const connectionString = process.env.AZURE_STORAGE_CONNECTION_STRING;
    if (!connectionString) {
        throw new Error('Azure Storage connection string not found');
    }

    const blobServiceClient = BlobServiceClient.fromConnectionString(connectionString);
    const containerClient = blobServiceClient.getContainerClient('ksports');
    
    const blobName = `${Date.now()}-${file.name}`;
    const blockBlobClient = containerClient.getBlockBlobClient(blobName);

    const arrayBuffer = await file.arrayBuffer();
    const buffer = Buffer.from(arrayBuffer);

    await blockBlobClient.upload(buffer, buffer.length, {
        blobHTTPHeaders: { blobContentType: file.type }
    });

    return blockBlobClient.url;
}

export async function uploadProductImage(formData: FormData) {
    const file = formData.get('file') as File;
    
    if (!file) {
        throw new Error('No file uploaded');
    }

    const validTypes = ['image/jpeg', 'image/png', 'image/webp'];
    if (!validTypes.includes(file.type)) {
        throw new Error('Invalid file type. Only JPEG, PNG and WebP allowed');
    }

    const maxSize = 5 * 1024 * 1024;
    if (file.size > maxSize) {
        throw new Error('File size too large. Maximum size is 5MB');
    }

    const url = await uploadToAzure(file);

    return {
        fileName: file.name,
        size: file.size,
        type: file.type,
        url
    };
}

export async function updateProduct(productId: number, formData: FormData) {
    const file = formData.get('file') as File
    let imageUrl

    if (file.size > 0) {
        const imageResult = await uploadProductImage(formData)
        imageUrl = imageResult.url
    }

    await prisma.products.update({
        where: { Id: productId },
        data: {
            ItemName: formData.get('itemName') as string,
            Description: formData.get('description') as string,
            Price: parseFloat(formData.get('price') as string),
            ...(imageUrl && { ImageSrc: imageUrl })
        }
    })

    revalidatePath('/admin')
    revalidatePath('/')

}


export async function deleteProduct(productId: number) {
    await prisma.products.delete({
        where: { Id: productId }
    })

    revalidatePath('/admin')
    revalidatePath('/')

}