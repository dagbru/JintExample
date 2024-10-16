

function generatePersons() {
    let persons = [];
    persons.push({
        FirstName: 'Per',
        LastName: 'Smith',
        BirthYear: 1977
    });

    persons.push({
        FirstName: 'Pål',
        LastName: 'Smith',
        BirthYear: 1990
    });

    persons.push({
        FirstName: 'Espen',
        LastName: 'Smith',
        BirthYear: 2001
    });
    
    persons.push(getDefaultPerson());
    
    persons.map(x => setLegalAge(x));
    
    return JSON.stringify(persons);
}

function setLegalAge(generatedPerson) {
    const now = new Date();
    const minAge = 30;
    generatedPerson.IsLegalAge = now.getFullYear() - generatedPerson.BirthYear > minAge;
}