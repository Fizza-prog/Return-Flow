
// Product Inspection: Select a request from the table
const inspectButtons = document.querySelectorAll("[data-inspect]");
const requestIdInput = document.getElementById("requestId");

inspectButtons.forEach(function (button) {
    button.addEventListener("click", function () {
        const requestId = button.getAttribute("data-inspect");

        requestIdInput.value = requestId;

        document.getElementById("inspection").scrollIntoView({
            behavior: "smooth"
        });

        requestIdInput.focus();
    });
});
