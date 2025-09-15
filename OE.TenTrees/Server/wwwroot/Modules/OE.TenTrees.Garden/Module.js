// Garden Monitoring Module JavaScript

window.GardenMonitoring = {
    // Initialize photo upload functionality
    initPhotoUpload: function () {
        const fileInput = document.querySelector('input[type="file"]');
        if (fileInput) {
            fileInput.addEventListener('change', function(e) {
                const file = e.target.files[0];
                if (file) {
                    // Validate file type
                    if (!file.type.startsWith('image/')) {
                        alert('Please select an image file.');
                        e.target.value = '';
                        return;
                    }
                    
                    // Validate file size (max 5MB)
                    if (file.size > 5 * 1024 * 1024) {
                        alert('File size must be less than 5MB.');
                        e.target.value = '';
                        return;
                    }
                    
                    // Show preview if needed
                    this.showImagePreview(file);
                }
            });
        }
    },

    // Show image preview
    showImagePreview: function(file) {
        const reader = new FileReader();
        reader.onload = function(e) {
            let preview = document.getElementById('image-preview');
            if (!preview) {
                preview = document.createElement('img');
                preview.id = 'image-preview';
                preview.style.maxWidth = '200px';
                preview.style.maxHeight = '200px';
                preview.style.marginTop = '10px';
                preview.className = 'img-thumbnail';
                
                const container = document.querySelector('input[type="file"]').parentNode;
                container.appendChild(preview);
            }
            preview.src = e.target.result;
        };
        reader.readAsDataURL(file);
    },

    // Calculate mortality rate
    calculateMortalityRate: function(planted, alive) {
        if (planted > 0 && alive !== null && alive !== undefined) {
            const dead = planted - alive;
            return (dead / planted * 100).toFixed(1);
        }
        return 0;
    },

    // Update mortality display
    updateMortalityDisplay: function() {
        const plantedInput = document.getElementById('treesPlanted');
        const aliveInput = document.getElementById('treesAlive');
        
        if (plantedInput && aliveInput) {
            const updateRate = () => {
                const planted = parseInt(plantedInput.value) || 0;
                const alive = parseInt(aliveInput.value) || 0;
                
                if (planted > 0) {
                    const rate = this.calculateMortalityRate(planted, alive);
                    let displayElement = document.getElementById('mortality-display');
                    
                    if (!displayElement) {
                        displayElement = document.createElement('div');
                        displayElement.id = 'mortality-display';
                        displayElement.className = 'alert mt-2';
                        aliveInput.parentNode.appendChild(displayElement);
                    }
                    
                    displayElement.innerHTML = `<strong>Mortality Rate:</strong> ${rate}%`;
                    
                    // Update alert class based on rate
                    displayElement.className = 'alert mt-2 ' + 
                        (rate > 30 ? 'alert-danger' : 
                         rate > 15 ? 'alert-warning' : 'alert-success');
                         
                    if (rate > 30) {
                        displayElement.innerHTML += ' <i class="fas fa-exclamation-triangle"></i> High mortality detected!';
                    }
                } else if (document.getElementById('mortality-display')) {
                    document.getElementById('mortality-display').style.display = 'none';
                }
            };
            
            plantedInput.addEventListener('input', updateRate);
            aliveInput.addEventListener('input', updateRate);
            
            // Initial calculation
            updateRate();
        }
    },

    // Initialize form validation
    initFormValidation: function() {
        const form = document.querySelector('form');
        if (form) {
            form.addEventListener('submit', function(e) {
                const applicationSelect = document.getElementById('applicationSelect');
                const beneficiaryInput = document.getElementById('beneficiaryName');
                
                if (applicationSelect && applicationSelect.value === '') {
                    e.preventDefault();
                    alert('Please select a garden/application to monitor.');
                    applicationSelect.focus();
                    return false;
                }
                
                if (beneficiaryInput && beneficiaryInput.value.trim() === '') {
                    e.preventDefault();
                    alert('Beneficiary name is required.');
                    beneficiaryInput.focus();
                    return false;
                }
            });
        }
    },

    // Initialize all functionality
    init: function() {
        this.initPhotoUpload();
        this.updateMortalityDisplay();
        this.initFormValidation();
        
        // Initialize tooltips if Bootstrap is available
        if (typeof bootstrap !== 'undefined') {
            const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
            tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl);
            });
        }
    }
};

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    window.GardenMonitoring.init();
});

// Re-initialize when Blazor updates the DOM
if (window.Blazor) {
    window.Blazor.addEventListener('enhancedload', function() {
        window.GardenMonitoring.init();
    });
}