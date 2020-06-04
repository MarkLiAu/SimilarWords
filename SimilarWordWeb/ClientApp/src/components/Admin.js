import React, { useState } from 'react';
import { GetTokenHeader, UserLogOut } from './CommTools';

const Admin = ({ cmd,location}) => {
    // Declare a new state variable, which we'll call "count"
    const [result, setResult] = useState('Processing');

    const CallUpdateApi = () => {
        //alert("in CallUpdateApi");
        console.log("admin CallUpdateApi");
        fetch('api/Admin/' + location.state.cmd, {
            method: 'PUT',
            body: "",
            headers: {
                'Content-Type': 'application/json',
                'Authorization': GetTokenHeader()
            }
        })
            .then(response => {
                console.log(response);
                if (!response.ok) throw new Error("Failed to load:(" + response.status.toString() + ") " + response.statusText);
                response.text()
            })
            .then(data => { console.log(data); setResult("Ok"); localStorage.setItem('SimilarWordUser', JSON.stringify(data) ) })
            .catch((error) => {
                console.error('Error:', error);
                setResult(error.message); 
            });
    }
    console.log(cmd);
    console.log(location.state.cmd);
    console.log(result);

    if (cmd === 'logout') {
        UserLogOut();
        setResult('You have logged out!');
    }
    else if (result === 'Processing') {
        if(window.confirm('Are you sure?')) CallUpdateApi();
    }

    return (
        <div>
            <h3>Admin</h3>
            {cmd} {result}
        </div>
    );
    

}


export default Admin;
