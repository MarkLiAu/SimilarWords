import React, { useState } from 'react';

const WordSearchInput = ({ initValue, handleSubmit }) => {
    let [inputData, setInputData] = useState(initValue);
    let [placeHolder, setPlaceHolder] = useState(initValue);

    const WordChanged = (e) => {
        setInputData(e.target.value);
    }

    const SubmitSearch = () => {
        console.log('WordSearchInput SubmitSearch');
        setPlaceHolder(inputData);
        handleSubmit(inputData);
    };

    const KeyPressed = (event) => {
        if (event.key === 'Enter') {
            SubmitSearch();
        }
    }

    return (
            <div className='bj_center'>
                    <input defaultValue={inputData} placeholder='search here' onChange={WordChanged} onKeyPress={KeyPressed} ></input>
                    <span>{' '}</span>
                    <button type="submit" className='btn btn-primary' onClick={SubmitSearch}><span className="glyphicon glyphicon-search"></span> </button>
            </div>
        );
}

export default WordSearchInput;



