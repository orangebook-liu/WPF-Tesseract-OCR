# 离线OCR文字识别工具 (OcrTool)

一款基于 WPF + Tesseract 的离线 OCR 文字识别桌面应用程序，支持中文、英文、数字等多种字符的离线识别。

## 功能特性

- **多种图片输入方式**
  - 手动选择图片文件（支持 PNG / JPG / JPEG / BMP）
  - 屏幕截图截取区域
  - 拖拽图片到软件窗口

- **离线使用** — 无需网络连接，完全本地运行

- **多语言识别** — 支持中文（简体）、英文、数字等常见字符

- **美观界面** — 现代化 UI 设计，遵循 MVVM 架构模式

- **便捷操作** — 一键复制识别结果到剪贴板

## 技术栈

| 技术 | 用途 |
|------|------|
| .NET 8.0 (WPF) | 桌面应用框架 |
| CommunityToolkit.Mvvm | MVVM 模式实现 |
| Tesseract 5.2 | OCR 文字识别引擎 |
| System.Drawing.Common | 屏幕截图与图像处理 |

## 项目结构

```
OcrTool/
├── App.xaml                 # 应用程序入口 & 全局样式资源
├── MainWindow.xaml          # 主窗口界面
├── MainWindow.xaml.cs       # 主窗口代码（拖拽事件处理）
├── OcrTool.csproj           # 项目配置文件
├── ViewModels/
│   └── MainViewModel.cs     # 主视图模型（MVVM 核心逻辑）
├── Services/
│   ├── OcrService.cs        # OCR 识别服务（Tesseract 封装）
│   ├── ScreenCapture.cs     # 屏幕截图服务
│   ├── CaptureForm.xaml     # 截图选区窗口
│   └── CaptureForm.xaml.cs  # 截图选区逻辑
├── Converters/
│   ├── NullToVisibilityConverter.cs   # 空值可见性转换器
│   ├── DragOverBrushConverter.cs      # 拖拽背景色转换器
│   └── DragOverBorderConverter.cs     # 拖拽边框色转换器
└── Resources/
    └── tessdata/            # Tesseract 语言训练数据包
        ├── chi_sim.traineddata  # 中文简体
        └── eng.traineddata      # 英文
```

## 快速开始

### 环境要求

- .NET 8.0 SDK 或更高版本
- Windows 操作系统

### 编译运行

```bash
# 还原依赖并编译
dotnet build

# 运行程序
dotnet run
```

或直接在 Visual Studio 中打开 `OcrTool.sln` 并按 F5 运行。

### 配置语言训练数据（必须）

OCR 识别功能需要 Tesseract 的语言训练数据包。请从以下地址下载：

| 语言 | 文件名 | 下载地址 |
|------|--------|----------|
| 中文简体 | `chi_sim.traineddata` | [GitHub](https://github.com/tesseract-ocr/tessdata/raw/main/chi_sim.traineddata) |
| 英文 | `eng.traineddata` | [GitHub](https://github.com/tesseract-ocr/tessdata/raw/main/eng.traineddata) |

下载后将文件放入 `Resources/tessdata/` 目录，然后重新编译项目即可。

> 如果只需要识别英文，可以只下载 `eng.traineddata`；如果需要同时支持中英文，则两个文件都需要。

## 使用说明

### 选择图片识别

1. 点击顶部「**选择图片**」按钮
2. 在弹出的文件对话框中选择图片文件
3. 图片加载后自动进行 OCR 识别
4. 右侧面板显示识别结果

### 截屏识别

1. 点击顶部「**截取屏幕**」按钮
2. 整个屏幕会变暗并显示当前画面
3. 按住鼠标左键拖动选择要识别的区域
4. 松开鼠标或按 **Enter** 确认，按 **Esc** 取消
5. 选定区域自动进行 OCR 识别

### 拖拽识别

1. 直接将图片文件拖入软件窗口左侧区域
2. 松开鼠标后自动加载并进行识别

### 其他操作

- **清空** — 清除当前图片和识别结果
- **复制结果** — 将识别文本复制到系统剪贴板

## MVVM 架构说明

本项目严格遵循 MVVM（Model-View-ViewModel）设计模式：

```
View (MainWindow.xaml)
    ↓ 绑定命令 & 数据
ViewModel (MainViewModel.cs)
    ↓ 调用服务接口
Service (IOcrService → OcrService.cs)
    ↓ 调用引擎
Tesseract OCR 引擎
```

- **View**: 仅负责 UI 展示，通过 Binding 绑定 ViewModel 的属性和命令
- **ViewModel**: 业务逻辑层，处理用户交互、管理状态，不直接引用 View
- **Service**: 底层服务封装，通过接口解耦，便于测试和替换

## 常见问题

### Q: 识别结果为空或报错？

请确认 `Resources/tessdata/` 目录下存在有效的 `.traineddata` 文件。空占位文件无法用于识别。

### Q: 截图功能没有反应？

截图功能依赖 `System.Drawing.Common` 中的屏幕捕获 API，确保以足够的权限运行程序。

### Q: 如何添加其他语言支持？

1. 从 [tessdata 仓库](https://github.com/tesseract-ocr/tessdata) 下载对应语言的 `.traineddata` 文件
2. 放入 `Resources/tessdata/` 目录
3. 在 `OcrService.cs` 中修改语言参数（如 `"chi_sim+eng"` 改为 `"chi_sim+eng+jpn"`）

## License

MIT License
