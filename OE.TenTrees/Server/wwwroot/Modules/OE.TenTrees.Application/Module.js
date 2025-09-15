// Application Module JavaScript

window.OETenTreesApplication = {
    
    // Initialize the module
    init: function() {
        console.log('Tree Planting Application module initialized');
        this.bindEvents();
    },

    // Bind events
    bindEvents: function() {
        // Auto-save draft functionality (optional)
        this.setupAutoSave();
        
        // Form validation helpers
        this.setupFormValidation();
    },

    // Setup auto-save for draft applications
    setupAutoSave: function() {
        let autoSaveTimer;
        const autoSaveInterval = 30000; // 30 seconds

        // Find form inputs and bind change events
        const formInputs = document.querySelectorAll('.application-form input, .application-form textarea, .application-form select');
        
        formInputs.forEach(input => {
            input.addEventListener('change', () => {
                clearTimeout(autoSaveTimer);
                autoSaveTimer = setTimeout(() => {
                    this.triggerAutoSave();
                }, autoSaveInterval);
            });
        });
    },

    // Trigger auto-save (would need to be connected to Blazor component)
    triggerAutoSave: function() {
        // This would typically invoke a Blazor method
        console.log('Auto-save triggered');
    },

    // Setup form validation helpers
    setupFormValidation: function() {
        // Add visual feedback for required fields
        const requiredInputs = document.querySelectorAll('input[required], textarea[required]');
        
        requiredInputs.forEach(input => {
            input.addEventListener('blur', () => {
                this.validateField(input);
            });
        });
    },

    // Validate individual field
    validateField: function(field) {
        const isValid = field.checkValidity();
        
        if (isValid) {
            field.classList.remove('is-invalid');
            field.classList.add('is-valid');
        } else {
            field.classList.remove('is-valid');
            field.classList.add('is-invalid');
        }
        
        return isValid;
    },

    // Confirm before navigating away from unsaved changes
    confirmNavigation: function(hasUnsavedChanges) {
        if (hasUnsavedChanges) {
            return confirm('You have unsaved changes. Are you sure you want to leave this page?');
        }
        return true;
    },

    // Show loading spinner
    showLoading: function(element) {
        if (element) {
            element.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Loading...';
            element.disabled = true;
        }
    },

    // Hide loading spinner
    hideLoading: function(element, originalText) {
        if (element) {
            element.innerHTML = originalText;
            element.disabled = false;
        }
    },

    // Format validation messages
    formatValidationMessage: function(fieldName, message) {
        return `${fieldName}: ${message}`;
    },

    // Calculate completion percentage
    calculateCompletionPercentage: function(formData) {
        const requiredFields = [
            'evaluatorName',
            'beneficiaryName'
        ];
        
        const optionalFields = [
            'householdIdentifier',
            'village',
            'householdSize'
        ];
        
        const commitmentFields = [
            'commitNoChemicals',
            'commitAttendTraining',
            'commitNotCutTrees',
            'commitStandAgainstAbuse'
        ];
        
        let completed = 0;
        let total = requiredFields.length + optionalFields.length + commitmentFields.length;
        
        // Check required fields
        requiredFields.forEach(field => {
            if (formData[field] && formData[field].trim() !== '') {
                completed++;
            }
        });
        
        // Check optional fields
        optionalFields.forEach(field => {
            if (formData[field] && formData[field].toString().trim() !== '') {
                completed++;
            }
        });
        
        // Check commitment fields
        commitmentFields.forEach(field => {
            if (formData[field] === true) {
                completed++;
            }
        });
        
        return Math.round((completed / total) * 100);
    }
};

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    window.OETenTreesApplication.init();
});