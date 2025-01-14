document.querySelectorAll('.setHidden').forEach(function (parent) {
    parent.addEventListener('click', function () {
        const childRows = parent.nextElementSibling;
        const child1Rows = parent.nextElementSibling?.nextElementSibling;

        if (!childRows || !child1Rows) {
            console.error('Child elements not found for parent:', parent);
            return;
        }

        if (childRows.classList.contains('d-none')) {
            childRows.classList.remove('d-none');
            child1Rows.classList.remove('d-none');
        } else {
            childRows.classList.add('d-none');
            child1Rows.classList.add('d-none');
        }
    });
});
document.querySelectorAll('.setMatchHidden').forEach(function (parent) {
    parent.addEventListener('click', function () {
        let nextSibling = parent.nextElementSibling;
        while (nextSibling && nextSibling.classList.contains('matchHidden')) {
 
            if (nextSibling.classList.contains('d-none')) {
                nextSibling.classList.remove('d-none');
               
         
            } else {
                nextSibling.classList.add('d-none');
               

            }
           nextSibling = nextSibling.nextElementSibling;
            
        }
    });
});

document.addEventListener("DOMContentLoaded", () => {
    for (let i = 1; i <= 6; i += 1) {
        const point1Input = document.getElementById(`point${i}`);
        const point2Input = document.getElementById(`point${i % 2 === 0 ? i - 1 : i + 1}`);
        const checkAndDisableInputs = () => {
            const point1Value = parseInt(point1Input.value) || 0;
            const point2Value = parseInt(point2Input.value) || 0;

            if ((point1Value >= 21 && Math.abs(point1Value - point2Value) >= 2) ||
                (point2Value >= 21 && Math.abs(point2Value - point1Value) >= 2)) {
                console.log(`Disabling inputs: point${i} and point${i % 2 === 0 ? i - 1 : i + 1}`);
                point1Input.style.pointerEvents = "none";
                point2Input.style.pointerEvents = "none";
            }
        };
        checkAndDisableInputs();
        point1Input.addEventListener("change", checkAndDisableInputs);
    }
});

