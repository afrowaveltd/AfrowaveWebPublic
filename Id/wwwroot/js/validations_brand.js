let brandNameOk = false;
let brandWebsiteOk = true;
let brandLogoOk = true;
let brandEmailOk = true;

let formOk = false;

const brandNameErr = document.getElementById("brand_err");
const brandWebsiteErr = document.getElementById("brand_url_err");
const brandLogoErr = document.getElementById("brand_logo_err");
const brandEmailErr = document.getElementById("brand_email_err");
const applicationIconErr = document.getElementById("brand_logo_err");

const iconPreview = document.getElementById("icon_preview");

const isValidUrl = (url) => {
    try {
        new URL(url);
        return true;
    } catch (e) {
        return false;
    }
};

const checkForm = () => {
    if (brandNameOk && brandWebsiteOk && brandLogoOk && brandEmailOk) {
        formOk = true;
    } else {
        formOk = false;
    }
    if (formOk) {
        document.getElementById("submit").removeAttribute("disabled");
    } else {
        document.getElementById("submit").setAttribute("disabled", "disabled");
    }
};

const checkBrand = async (element) => {
    const brandName = element.value;
    if (brandName.length < 2) {
        element.classList.remove("input-valid");
        element.classList.add("input-invalid");
        brandNameErr.innerHTML = await localize(
            "Brand name must be at least 2 characters long"
        );
        brandNameOk = false;
        checkForm();
        return;
    }
    if (brandName.length > 50) {
        element.classList.remove("input-valid");
        element.classList.add("input-invalid");
        brandNameErr.innerHTML = await localize(
            "The brand name must be shorter than 50 characters"
        );
        brandNameOk = false;
        checkForm();
        return;
    }
    // check if is brand name unique
    const result = await fetchData("/api/IsBrandNameUnique/" + brandName);
    if (result.success) {
        if (result.data.isUnique) {
            element.classList.remove("input-invalid");
            element.classList.add("input-valid");
            brandNameErr.innerHTML = "";
            brandNameOk = true;
            checkForm();
            return;
        } else {
            element.classList.remove("input-valid");
            element.classList.add("input-invalid");
            brandNameErr.innerHTML = await localize(
                "Brand name is already registered"
            );
            brandNameOk = false;
            checkForm();
            return;
        }
    } else {
        element.classList.remove("input-valid");
        element.classList.add("input-invalid");
        brandNameErr.innerHTML = await localize("Error checking brand name");
        brandNameOk = false;
        checkForm();
        return;
    }
};

const checkUrl = async (element) => {
    const brandUrl = element.value;
    if (brandUrl.length > 0) {
        if (!isValidUrl(brandUrl)) {
            console.log("Invalid URL");
            element.classList.remove("input-valid");
            element.classList.add("input-invalid");
            brandWebsiteErr.innerHTML = await localize("Invalid URL");
            brandWebsiteOk = false;
            checkForm();
            return;
        } else {
            element.classList.remove("input-invalid");
            element.classList.add("input-valid");
            brandWebsiteErr.innerHTML = "";
            brandWebsiteOk = true;
            checkForm();
            return;
        }
    } else {
        console.log("Empty URL");
        element.classList.remove("input-invalid");
        element.classList.add("input-valid");
        brandWebsiteErr.innerHTML = "";
        brandWebsiteOk = true;
        checkForm();
        return;
    }
};

const checkIcon = async (element) => {
    console.log("Checking the icon");
    const validTypes = ["image/jpeg", "image/png", "image/jpg", "image/gif"];
    const file = element.files[0];
    if (!file) {
        console.log("File not found");

        element.classList.add("input-valid");
        element.classList.remove("input-invalid");
        applicationIconOk = true;
        applicationIconErr.innerHTML = "";
        checkForm();
        return;
    }
    console.log(file);
    if (validTypes.includes(file.type)) {
        try {
            const isValidImage = await checkIfRealImage(file);
            console.log(isValidImage);
            if (!isValidImage) {
                element.classList.remove("input-valid");
                element.classList.add("input-invalid");
                applicationNameErr.innerHTML = await localize(
                    "Error checking application name"
                );
                applicationNameOk = false;
                checkForm();
                message.textContent = "The file is not a valid image.";
            } else {
                showImagePreview(file); // Display the image preview
                element.classList.remove("input-invalid");
                element.classList.add("input-valid");
                applicationIconErr.innerHTML = "";
                applicationIconOk = true;
                checkForm();
            }
        } catch (error) {
            message.textContent = "An error occurred while validating the file.";
        }
    }

    /**
     * Check if a file is a real image
     * @param {File} file
     * @returns {Promise<boolean>}
     */
    async function checkIfRealImage(file) {
        const reader = new FileReader();

        return new Promise((resolve, reject) => {
            reader.onload = (e) => {
                const img = new Image();
                img.onload = () => resolve(true); // Real image
                img.onerror = () => resolve(false); // Invalid image
                img.src = e.target.result; // Load the image
            };

            reader.onerror = () => reject("Error reading the file");
            reader.readAsDataURL(file); // Read file content as Data URL
        });
    }

    /**
     * Show a 32x32px preview of the uploaded image
     * @param {File} file
     */
    async function showImagePreview(file) {
        const reader = new FileReader();

        reader.onload = (e) => {
            const img = document.createElement("img");
            img.src = e.target.result; // Set the source to the file's data URL
            img.alt = "Image Preview";
            iconPreview.innerHTML = "";
            iconPreview.appendChild(img);
        };

        reader.readAsDataURL(file); // Read file content as Data URL
    }
};

const checkMail = async (element) => {
    console.log("checkEmail");
    // validate format
    const email = element.value;
    let emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (!emailRegex.test(email)) {
        element.classList.remove("input-valid");
        element.classList.add("input-invalid");
        brandEmailErr.innerHTML = await localize("Invalid email address");
        emailOk = false;
    } else {
        element.classList.remove("input-invalid");
        element.classList.add("input-valid");
        brandEmailErr.innerHTML = "";
        emailOk = true;
    }
    checkForm();
};