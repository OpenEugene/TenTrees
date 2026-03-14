---
priority: medium
tags: []
---

# Design: Create logo and favicon for 10 Trees platform

## 🎨 Design Request

The 10 Trees Digital Platform needs a logo and favicon to establish brand identity and improve user experience.

## 📋 Required Assets

### 1. **Logo (Primary)**
- **Purpose**: Website header, documentation, marketing materials
- **Formats Needed**:
  - SVG (scalable vector - preferred)
  - PNG (transparent background)
    - 512x512px (high-res)
    - 256x256px (standard)
    - 128x128px (small)
- **Usage**:
  - Oqtane theme header
  - Login page
  - Admin dashboard
  - Email templates
  - Documentation

### 2. **Favicon Package**
- **Purpose**: Browser tabs, mobile home screen, PWA icons
- **Formats Needed**:
  - `favicon.ico` (16x16, 32x32, 48x48 multi-size)
  - `favicon-16x16.png`
  - `favicon-32x32.png`
  - `apple-touch-icon.png` (180x180)
  - `android-chrome-192x192.png`
  - `android-chrome-512x512.png`
  - `site.webmanifest` (PWA manifest)

### 3. **Logo Variants** (Optional but Recommended)
- Full logo (with text)
- Icon only (square/circular)
- Horizontal layout
- Stacked layout
- Light theme version
- Dark theme version

## 🎯 Design Considerations

### Brand Elements
- **Project Name**: 10 Trees Digital Platform
- **Mission**: Supporting rural South African communities through permaculture and tree planting
- **Target Users**: 
  - Rural mentors (mobile-first)
  - Program coordinators
  - Village participants
  - Donors/stakeholders

### Design Direction
- **Nature/Environmental**: Reflect tree planting, sustainability, growth
- **Community**: Represent South African communities, togetherness
- **Simplicity**: Mobile-friendly, works at small sizes
- **Cultural Sensitivity**: Appropriate for South African context
- **Color Palette**: 
  - Earthy tones (greens, browns)
  - Vibrant but professional
  - High contrast for accessibility

### Technical Requirements
- **Scalable**: Must work from 16x16px to large print sizes
- **Distinctive**: Recognizable at small sizes (mobile icons)
- **Accessible**: WCAG AA compliant color contrast
- **File Size**: Optimized for web (especially for mobile/offline use)
- **Cross-Platform**: Works on Android, iOS, Windows, web browsers

## 📁 File Locations in Project

### Logo Files
```
Server/wwwroot/images/
├── logo.svg                    # Primary logo (vector)
├── logo-512.png               # High-res PNG
├── logo-256.png               # Standard PNG
├── logo-128.png               # Small PNG
├── logo-icon.svg              # Icon-only version
└── logo-horizontal.svg        # Horizontal layout
```

### Favicon Files
```
Server/wwwroot/
├── favicon.ico                # Classic favicon
├── favicon-16x16.png
├── favicon-32x32.png
├── apple-touch-icon.png       # iOS home screen
├── android-chrome-192x192.png # Android
├── android-chrome-512x512.png # Android high-res
└── site.webmanifest          # PWA manifest
```

### Usage in Code

**HTML Head (wwwroot/index.html or _Host.cshtml):**
```html





```

**Theme Header (Client/Themes/TenTreesTheme/Theme1.razor):**
```razor
<img src="~/images/logo-horizontal.svg" alt="10 Trees"/>
```

## 🎨 Inspiration & References

### Concepts to Consider
- **Trees**: Indigenous South African trees (Marula, Baobab, Acacia)
- **Numbers**: Incorporate "10" subtly
- **Growth**: Seedling to mature tree progression
- **Community**: Multiple trees together (forest/grove)
- **Hands**: Planting, nurturing, growing

### Similar Projects for Reference
- One Tree Planted (https://onetreeplanted.org)
- Trees for the Future (https://trees.org)
- African conservation logos

## ✅ Acceptance Criteria

- [ ] Logo design approved by project stakeholders
- [ ] All required file formats provided
- [ ] Logo works at all sizes (16px to print)
- [ ] Favicon displays correctly in all major browsers
- [ ] PWA icons work on Android/iOS home screens
- [ ] Files optimized for web delivery
- [ ] Design guidelines/brand kit provided
- [ ] Files committed to repository
- [ ] Updated in Oqtane theme
- [ ] Documented in README.md

## 📊 Priority

**High** - Important for professional appearance and user recognition, especially for mobile app experience

## 🔗 Related Files

- `Server/wwwroot/index.html` or `_Host.cshtml`
- `Client/Themes/TenTreesTheme/Theme1.razor`
- `Client/Themes/TenTreesTheme/ThemeInfo.cs`
- `Server/wwwroot/Themes/OpenEug.TenTrees.Theme.TenTreesTheme/Theme.css`
- `README.md`

## 💡 Additional Notes

### Designer Resources
- Project repository: https://github.com/OpenEugene/10Trees
- Target deployment: MonsterASP (tentrees.org)
- Offline-first mobile app for rural South Africa
- Bilingual: English and Xitsonga

### Temporary Solution
Until logo is created, use:
- Text-based header: "10 Trees"
- Default Oqtane branding
- Placeholder favicon

---

**Timeline**: Needed before public launch  
**Contact**: Project coordinator for design approval  
**Budget**: TBD (volunteer designer or paid contract)
