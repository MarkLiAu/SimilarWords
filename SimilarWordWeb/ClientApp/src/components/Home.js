import React, { useState } from 'react';
import {Link} from 'react-router-dom'

export const Home = () => {
    const [wordInput, setWordInput] = useState('');

    return (
      <div>
        <h1>Hello!</h1>
            <input defaultValue={wordInput} placeholder='search here' onChange={WordChanged} onKeyPress={this.KeyPressed} ></input>
            <Link to={'/wordsearch/' + wordInput} > <button>Search</button> </Link>
      </div>
    );

    const WordChanged = (e) => {
        setWordInput(e.target.value);
    }

}
