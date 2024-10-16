

function updatePerson() {
    const now = new Date();
    const minAge = 50;
    person.IsLegalAge = now.getFullYear() - person.BirthYear > minAge;
}