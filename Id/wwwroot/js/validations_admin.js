// installation form

let emailOk = false;
let passwordOk = false;
let confirmPasswordOk = false;

let formOk = false;

const emailErr = document.getElementById('email_err');
const passwordErr = document.getElementById('password_err');
const confirmPasswordErr = document.getElementById('password_confirm_err');

const checkForm = () => {
    if (emailOk && passwordOk && confirmPasswordOk) {
        formOk = true;
    } else {
        formOk = false;
    }
    if (formOk) {
        document.getElementById('submit').removeAttribute('disabled');
    }
    else {
        document.getElementById('submit').setAttribute('disabled', 'disabled');
    }
};

const testEmail = async (element) => {
    console.log("checked");
}

const checkPassword = async (password, element) => {
    let passwordErrors = [];
    // to do when password rules are set up

    if (passwordErrors.length > 0) {
        element.classList.remove('input-valid');
        element.classList.add('input-invalid');
        passwordErr.innerHTML = passwordErrors.join('<br>');
        passwordOk = false;
    }
    else {
        element.classList.remove('input-invalid');
        element.classList.add('input-valid');
        passwordErr.innerHTML = "";
        passwordOk = true;
    }
    checkForm();
};

const checkPasswordConfirm = async (password, confirmPassword, element) => {
    if (password != confirmPassword) {
        element.classList.remove('input-valid');
        element.classList.add('input-invalid');
        confirmPasswordErr.innerHTML = await localize("Passwords do not match");
        confirmPasswordOk = false;
    }
    else {
        element.classList.remove('input-invalid');
        element.classList.add('input-valid');
        confirmPasswordErr.innerHTML = "";
        confirmPasswordOk = true;
    }
    checkForm();
};

const checkMail = async (element) => {
    console.log('checkEmail');
    // validate format
    const email = element.value;
    let emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (!emailRegex.test(email)) {
        element.classList.remove('input-valid');
        element.classList.add('input-invalid');
        emailErr.innerHTML = await localize("Invalid email address");
        emailOk = false;
    }
    else {
        // check if is email unique
        const result = await fetchData('/api/IsEmailUnique/' + email);
        if (result.success) {
            if (result.data.isUnique) {
                element.classList.remove('input-invalid');
                element.classList.add('input-valid');
                emailErr.innerHTML = "";
                emailOk = true;
            }
            else {
                element.classList.remove('input-valid');
                element.classList.add('input-invalid');
                emailErr.innerHTML = await localize("Email address is already registered");
                emailOk = false;
            }
        }
        else {
            element.classList.remove('input-valid');
            element.classList.add('input-invalid');
            emailErr.innerHTML = await localize("Error checking email address");
            emailOk = false;
        }
    }
    checkForm();
};