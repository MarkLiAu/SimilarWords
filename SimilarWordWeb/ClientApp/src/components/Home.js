import React, { useState } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { loginUser } from './CommTools';
import WordSearchInput  from './WordSearchInput';

export const Home = ({ history }) => {

    const handleWordInout = (word) => {
        console.log('home handlewordinput:' + word);
        history.push('/Wordsearch/' + word);
    }
        return (
            <div>
                <h3>Welcome {loginUser===null? 'Guest': loginUser.firstName}!          </h3>
                <Link to='/wordsearch'><button className='btn btn-outline'>Search</button></Link><br/>
                <Link to='/wordmemory'><button className='btn btn-outline'>Memory</button></Link>
                <WordSearchInput handleSubmit={handleWordInout} ></WordSearchInput>
            </div>
        );

}
