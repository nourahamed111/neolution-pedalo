window.addEventListener('DOMContentLoaded', (event) => {
    const pedaloSelect = document.getElementById('Booking_PedaloName');
    const pedaloCapacityTextBox = document.getElementById('pedaloCapacityTextBox');
    const checkboxes = document.querySelectorAll('.passenger-checkbox');
    const editButton = document.getElementById('editButton');
    const validationText = document.getElementById('validationText');

    const updatePedaloCapacity = () => {
        const selectedOption = pedaloSelect.options[pedaloSelect.selectedIndex];
        const pedaloCapacity = parseInt(selectedOption.getAttribute('data-capacity'));
        pedaloCapacityTextBox.value = pedaloCapacity || '';
    };

    const updateValidation = () => {
        const selectedCheckboxCount = document.querySelectorAll('.passenger-checkbox:checked').length;
        const pedaloCapacity = parseInt(pedaloCapacityTextBox.value);
        const remainingCapacity = pedaloCapacity - 1;

        if (selectedCheckboxCount > remainingCapacity) {
            editButton.disabled = true;
            validationText.textContent = `The limit of capacity is ${remainingCapacity}. You can't add more passengers.`;
        } else {
            editButton.disabled = false;
            validationText.textContent = '';
        }
    };

    pedaloSelect.addEventListener('change', () => {
        updatePedaloCapacity();
        updateValidation();
    });

    checkboxes.forEach((checkbox) => {
        checkbox.addEventListener('change', () => {
            updateValidation();
        });
    });

    updatePedaloCapacity();
    updateValidation();
});









