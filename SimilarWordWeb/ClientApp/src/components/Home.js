import React, { useState } from 'react';
import { Link } from 'react-router-dom'
import { Badge } from 'react-bootstrap';
import { WordSearch } from './WordSearch';
import { loginUser } from './CommTools';


export const Home = ({history}) => {

        return (
            <div>
                <h3>Welcome {loginUser===null? 'Guest': loginUser.firstName}!          </h3>
                <Link to='/wordsearch'><button className='btn btn-outline'>Search</button></Link><br/>
                <Link to='/wordmemory'><button className='btn btn-outline'>Memory</button></Link>
            </div>
        );

}
