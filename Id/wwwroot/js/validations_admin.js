// Installation Form Validation

/**
 * Flags indicating whether the email, password, and password confirmation are valid.
 * Used to determine if the form can be submitted.
 * @type {boolean}
 */
let emailOk = false;
let passwordOk = false;
let confirmPasswordOk = false;

/**
 * Flag indicating whether the form is valid for submission.
 * @type {boolean}
 */
let formOk = false;

/**
 * References to error message elements for form validation feedback.
 */
const emailErr = document.getElementById('email_err');
const passwordErr = document.getElementById('password_err');
const confirmPasswordErr = document.getElementById('password_confirm_err');

/**
 * Checks the overall validity of the form based on individual field validations.
 * Enables or disables the submit button accordingly.
 */
const checkForm = () => {
    if (emailOk && passwordOk && confirmPasswordOk) {
        formOk = true;
    } else {
        formOk = false;
    }

    if (formOk) {
        document.getElementById('submit').removeAttribute('disabled');
    } else {
        document.getElementById('submit').setAttribute('disabled', 'disabled');
    }
};

/**
 * Placeholder function to test email input.
 * @param {HTMLElement} element - The email input element.
 * @returns {Promise<void>}
 */
const testEmail = async (element) => {
    console.log("checked");
};

/**
 * Validates the provided password against defined password rules.
 * Updates UI feedback accordingly.
 * @param {string} password - The password input value.
 * @param {HTMLElement} element - The password input element.
 * @returns {Promise<void>}
 */
const checkPassword = async (password, element) => {
    let passwordErrors = [];

    // TODO: Implement password rules validation

    if (passwordErrors.length > 0) {
        element.classList.remove('input-valid');
        element.classList.add('input-invalid');
        passwordErr.innerHTML = passwordErrors.join('<br>');
        passwordOk = false;
    } else {
        element.classList.remove('input-invalid');
        element.classList.add('input-valid');
        passwordErr.innerHTML = "";
        passwordOk = true;
    }

    checkForm();
};

/**
 * Validates whether the confirmed password matches the original password.
 * Updates UI feedback accordingly.
 * @param {string} password - The original password input value.
 * @param {string} confirmPassword - The confirmed password input value.
 * @param {HTMLElement} element - The password confirmation input element.
 * @returns {Promise<void>}
 */
const checkPasswordConfirm = async (password, confirmPassword, element) => {
    if (password !== confirmPassword) {
        element.classList.remove('input-valid');
        element.classList.add('input-invalid');
        confirmPasswordErr.innerHTML = await localize("Passwords do not match");
        confirmPasswordOk = false;
    } else {
        element.classList.remove('input-invalid');
        element.classList.add('input-valid');
        confirmPasswordErr.innerHTML = "";
        confirmPasswordOk = true;
    }

    checkForm();
};

/**
 * Validates the email format and checks if the email is unique.
 * Provides feedback based on the validation results.
 * @param {HTMLElement} element - The email input element.
 * @returns {Promise<void>}
 */
const checkMail = async (element) => {
    console.log('checkEmail');

    // Retrieve the email input value
    const email = element.value;

    // Regular expression for basic email format validation
    let emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;

    // Check if email format is valid
    if (!emailRegex.test(email)) {
        element.classList.remove('input-valid');
        element.classList.add('input-invalid');
        emailErr.innerHTML = await localize("Invalid email address");
        emailOk = false;
    } else {
        // Check if the email is unique via API request
        const result = await fetchData('/api/IsEmailUnique/' + email);

        if (result.success) {
            if (result.data.isUnique) {
                element.classList.remove('input-invalid');
                element.classList.add('input-valid');
                emailErr.innerHTML = "";
                emailOk = true;
            } else {
                element.classList.remove('input-valid');
                element.classList.add('input-invalid');
                emailErr.innerHTML = await localize("Email address is already registered");
                emailOk = false;
            }
        } else {
            element.classList.remove('input-valid');
            element.classList.add('input-invalid');
            emailErr.innerHTML = await localize("Error checking email address");
            emailOk = false;
        }
    }

    checkForm();
};
