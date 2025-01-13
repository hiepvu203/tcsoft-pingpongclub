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

