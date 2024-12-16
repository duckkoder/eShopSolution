from fastapi import FastAPI
from pydantic import BaseModel
from fastapi.responses import JSONResponse
import tensorflow as tf
import numpy as np

# Tải mô hình .h5
app = FastAPI()
model = tf.keras.models.load_model(r'D:\Code\eShopSolution1\FastapiServer\clothing_size_model.h5')

# Hàm ánh xạ lại từ số sang size
reverse_size_mapping = {0: 'XS', 1: 'S', 2: 'M', 3: 'L', 4: 'XL', 5: 'XXL'}

# Ánh xạ Type (Category) sang số
type_mapping = {
    'Blazer': 0,
    'Jumpsuit': 1,
    'Quần jean': 2,
    'Quần kaki': 3,
    'Quần short': 4,
    'Sweater': 5,
    'Váy': 6,
    'Áo khoác': 7,
    'Áo sơ mi': 8,
    'Áo thun': 9,
    'T-shirt':1,
    'Jacket':7,
    'Short':4,
    'Dresses':6
}

# Ánh xạ Brand sang số
brand_mapping = {
    'Adidas': 0,
    'Gucci': 1,
    'H&M': 2,
    'Hermes': 3,
    'Levi\'s': 4,
    'Nike': 5,
    'Prada': 6,
    'Puma': 7,
    'Uniqlo': 8,
    'Zara': 9
}

# Định nghĩa Request Model
class ProductSizePredictRequest(BaseModel):
    Height: float
    Weight: float
    Brand: str  # Thay đổi thành str vì Brand sẽ được chuyển đổi từ tên thành số
    Type: str  # Thay đổi thành str vì Type sẽ được chuyển đổi từ tên thành số
    Gender: int  # 0: nữ, 1: nam

@app.post("/predict")
async def predict(request: ProductSizePredictRequest):
    try:
        # Ánh xạ Type và Brand từ tên sang số
        print(request.Brand, request.Type, request.Gender, request.Height, request.Weight)

        brand_num = brand_mapping.get(request.Brand)
        type_num = type_mapping.get(request.Type)
        print(brand_num, type_num)
        if brand_num is None or type_num is None:
            return JSONResponse(content={'error': 'Invalid brand or type'}, status_code=400)

        # Chuyển đổi dữ liệu thành dạng NumPy array
        input_data = np.array([[request.Height, request.Weight, brand_num, type_num, request.Gender]])

        # Dự đoán
        predictions = model.predict(input_data)

        # Lấy index của kích thước có xác suất cao nhất
        predicted_size_index = np.argmax(predictions)

        # Trả về kết quả
        return JSONResponse(content={'PredictedSize': reverse_size_mapping[predicted_size_index]})

    except Exception as e:
        return JSONResponse(content={'error': str(e)}, status_code=400)

if __name__ == '__main__':
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
