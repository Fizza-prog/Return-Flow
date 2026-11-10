//Login form
const loginForm = document.getElementById("loginForm");

if (loginForm) {

    loginForm.addEventListener("submit", function (event) {

        event.preventDefault();

        const role = document.getElementById("userRole").value;

        const dashboards = {
            customer: "/customer-dashboard.html",
            manager: "/manager-dashboard.html",
            admin: "/admin-dashboard.html"
        };

        if (dashboards[role]) {
            window.location.href = dashboards[role];
        }

    });

}


//Signup Form
const signupForm = document.getElementById("signupForm");

if (signupForm) {

    signupForm.addEventListener("submit", function (event) {

        event.preventDefault();

        const passwords = signupForm.querySelectorAll(
            'input[type="password"]'
        );

        if (passwords[0].value !== passwords[1].value) {
            alert("Passwords do not match!");
            return;
        }

        alert("Demo signup completed! You can now login.");

        window.location.href = "/login.html";

    });

}


// Demo Return Request Form
const returnForm = document.getElementById("returnForm");

if (returnForm) {

    returnForm.addEventListener("submit", function (event) {

        event.preventDefault();

        alert("Demo: Return request submitted successfully!");

        returnForm.reset();

    });

}


// Demo Inspection Form
const inspectionForm = document.getElementById("inspectionForm");

if (inspectionForm) {

    inspectionForm.addEventListener("submit", function (event) {

        event.preventDefault();

        alert("Demo: Inspection details saved successfully!");

        inspectionForm.reset();

    });

}
