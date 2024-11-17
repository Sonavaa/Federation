"use strict";

const addInputBtn = document.getElementById("addInputBtn");
const divContainers = document.getElementsByClassName("box");

addInputBtn.addEventListener("click", () => {
  
  Array.from(divContainers).forEach(divContainer => {
    const newDiv = document.createElement("div");
    newDiv.id = "inputContainer";
    newDiv.className = "input";

    const newInput = document.createElement("input");
    newInput.type = "text";
    newInput.placeholder = "Enter value";
    newInput.className = "new-input";

    newDiv.appendChild(newInput);
    divContainer.insertBefore(newDiv, addInputBtn); 
  });
});