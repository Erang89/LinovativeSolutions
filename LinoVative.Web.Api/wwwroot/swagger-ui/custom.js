(function () {
    window.addEventListener("load", function () {
        const observer = new MutationObserver(() => {
            const title = document.querySelector(".title");
            if (title && title.innerText.includes("Linovative Public V1.0")) {
                document.body.classList.add("public-api");
            } else {
                document.body.classList.remove("public-api");
            }
        });

        observer.observe(document.body, { childList: true, subtree: true });
    });
})();
