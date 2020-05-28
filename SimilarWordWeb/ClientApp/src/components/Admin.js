import React, { Fragment, useState } from 'react';
import { Form, FormControl, FormGroup, ControlLabel, Col, Button, Label } from 'react-bootstrap';

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
                'Content-Type': 'application/json'
            }
        })
            .then(response => response.text() )
            .then(data => { console.log(data); setResult(data); })
    }
    console.log(cmd);
    console.log(location.state.cmd);
    console.log(result);
    if (result === 'Processing') CallUpdateApi();

    return (
        <div>
            <h3>Admin</h3>
            {cmd} {result}
        </div>
    );
    

}


export default Admin;
