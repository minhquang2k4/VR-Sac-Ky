# 🧪 **Hệ thống mô phỏng thí nghiệm Sắc ký lớp mỏng (TLC) bằng công nghệ VR**

Ứng dụng chạy trên Meta Quest -- phát triển bằng Unity, Meta XR SDK và
Firebase.

------------------------------------------------------------------------

## 📌 Giới thiệu

Hệ thống mô phỏng thí nghiệm **Sắc ký lớp mỏng (TLC)** trong môi trường
thực tế ảo (VR), giúp sinh viên thực hành quy trình phòng thí nghiệm một
cách trực quan, an toàn và không giới hạn số lần luyện tập.\
Ứng dụng chạy độc lập trên kính **Meta Quest**, tương tác đầy đủ bằng
controller hoặc hand-tracking, và lưu dữ liệu học tập qua Firebase.

------------------------------------------------------------------------

# 📂 **Cấu trúc thư mục dự án**

Dựa trên cấu trúc thực tế bạn đang sử dụng:

    Project/
    ├── Assets/
    │   ├── 3D Laboratory Environment with Apppratus/     # Môi trường phòng thí nghiệm 3D
    │   ├── CurvedUI/                                     # UI dạng cong tối ưu cho VR
    │   ├── HandGrabInteractableDataCollection/           # Dữ liệu grab & hand tracking
    │   ├── Image/                                        # Ảnh, texture
    │   ├── Oculus/                                       # Meta/Oculus SDK chính thức
    │   ├── Plugins/                                      # Plugin bên thứ 3
    │   ├── ProBuilder Data/                              # Dữ liệu từ ProBuilder
    │   ├── Resources/                                    # File load runtime
    │   ├── Samples/                                      # Tài nguyên mẫu từ package
    │   ├── Scenes/                                       # Scene chính của ứng dụng
    │   ├── ScenesLab/                                    # Scene lab thử nghiệm
    │   ├── Script/                                       # Mã nguồn C#
    │   │   ├── TLCSteps/                                 # Quy trình TLC theo từng bước
    │   │   ├── Interaction/                              # Grab, ray, collider, VR input
    │   │   ├── UI/                                       # Hệ thống giao diện VR
    │   │   └── Firebase/                                 # Ghi dữ liệu & đồng bộ với cloud
    │   ├── TextMesh Pro/                                 # Text trong giao diện
    │   ├── TutorialInfo/                                 # Thư mục auto từ Unity
    │   ├── XR/                                           # XR Plugin Management
    │   ├── XRI/                                          # XR Interaction Toolkit
    │   ├── dungcu/                                       # Dụng cụ thí nghiệm TLC
    │   └── msVFX_Free Smoke Effects Pack/                # Hiệu ứng VFX
    │
    ├── ProjectSettings/                                  # Cấu hình Unity
    ├── Packages/                                         # Package manifest Unity
    └── README.md                                         # File mô tả (file này)

------------------------------------------------------------------------

# 🧬 **Chức năng chính**

### ✔ Mô phỏng phòng thí nghiệm 3D chân thực

Sử dụng mô hình 3D từ `3D Laboratory Environment with Apppratus` và thư
mục `dungcu`.

### ✔ Quy trình TLC hoàn chỉnh

Bao gồm: - Chuẩn bị bản mỏng\
- Chấm mẫu\
- Pha chế pha động\
- Triển khai sắc ký\
- Lấy bản mỏng và đánh dấu\
- Quan sát & phân tích kết quả

Toàn bộ logic quản lý trong:\
`Assets/Script/TLCSteps/`

### ✔ Tương tác VR tự nhiên

-   Cầm nắm vật thể (Grab)\
-   Ray interaction\
-   Hand-tracking (nếu bật)\
-   Vật lý va chạm thực tế

Hệ thống nằm trong:\
`Assets/Script/Interaction/`

### ✔ Lưu dữ liệu học tập qua Firebase

Firebase lưu: - Thời gian thực hành\
- Số lỗi thao tác\
- Các bước hoàn thành\
- Hành vi tương tác của sinh viên

Module Firebase nằm trong:\
`Assets/Script/Firebase/`

------------------------------------------------------------------------

# ⚙ **Yêu cầu phần mềm**

-   Unity **2022/2023 LTS**
-   Meta XR SDK / Oculus Integration
-   XR Interaction Toolkit
-   Firebase SDK for Unity
-   Android Build Support

------------------------------------------------------------------------

# 🎮 **Cách build ứng dụng**

### 1. Mở project bằng Unity Hub

Unity sẽ tự load plugin Oculus/Meta XR và XR Interaction Toolkit.

### 2. Chuyển nền tảng sang Android

**File → Build Settings → Android → Switch Platform**

### 3. Cài lên Meta Quest

-   **Build & Run** trực tiếp\
    hoặc\
-   Xuất APK → Cài bằng **SideQuest**

------------------------------------------------------------------------

# 🧑‍🏫 **Dành cho giảng viên**

-   Đánh giá thao tác theo thời gian\
-   Hỗ trợ phân tích hành vi học tập

------------------------------------------------------------------------

# 📝 **Tác giả**

**Phạm Minh Quân**\
Đại học Kinh tế Quốc dân --- Khoa Công nghệ thông tin

------------------------------------------------------------------------

