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

for (let i = 1; i <= 6; i += 1) {
    if (i % 2 === 0) {
        document.getElementById(`point${i}`).addEventListener("change", () => {
            console.log("hihi")
            const point1 = document.getElementById(`point${i}`);
            const point2 = document.getElementById(`point${i - 1}`);

            const point1Value = parseInt(point1.value) || 0;
            const point2Value = parseInt(point2.value) || 0;

            if ((point1Value === 21 && Math.abs(point1Value - point2Value) >= 2) ||
                (point2Value === 21 && Math.abs(point2Value - point1Value) >= 2)) {

                document.getElementById(`point${i}`).disabled = true;
     
                document.getElementById(`point${i - 1}`).disabled = true;
            }
        });
    }

    else {
        document.getElementById(`point${i}`).addEventListener("change", () => {
            console.log("hahah")
        const point1 = document.getElementById(`point${i}`);
        const point2 = document.getElementById(`point${i + 1}`);

        const point1Value = parseInt(point1.value) || 0; 
        const point2Value = parseInt(point2.value) || 0;

        if ((point1Value === 21 && Math.abs(point1Value - point2Value) >= 2) ||
            (point2Value === 21 && Math.abs(point2Value - point1Value) >= 2)) {
          
            document.getElementById(`point${i}`).disabled = true;
            document.getElementById(`point${i + 1   }`).disabled = true;
        }
    });
    }
    
}
