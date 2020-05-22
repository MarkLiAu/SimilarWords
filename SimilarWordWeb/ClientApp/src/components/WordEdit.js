import React, { Fragment, useState } from 'react';
import { Form, FormControl, FormGroup, ControlLabel, Col, Button, Label } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const EditWord = ({ name, location }) => {
    // Declare a new state variable, which we'll call "count"
    const [word, setWord] = useState(location.state.word);
    const [result, setResult] = useState('');

    const CallUpdateApi = (e) => {
        //alert("in CallUpdateApi");
        console.log("CallUpdateApi");
        console.log(word);
        console.log(JSON.stringify(word));
        e.preventDefault();
        fetch('api/Words/'+word.name, {
            method: 'PUT',
            body: JSON.stringify(word),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => response.json())
            .then(json => { console.log(json); setResult('update finished'); })
    }

    const CallDeleteApi = (e) => {
        //alert("in CallUpdateApi");
        e.preventDefault();
        fetch('api/Words/' + word.name, {
            method: 'DELETE',
            body: JSON.stringify(word),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => response.json())
            .then(json => { console.log(json); setResult('delete finished'); })
    }



    const FormField = ({ type, label, name, val, onChangeHandle }) => {
        return (
            <FormGroup controlId={"form" + label}>
                <Col componentClass={ControlLabel} sm={2}>
                    {label}
                </Col>
                <Col sm={8}>
                    {type === 'text' ?
                        <FormControl type={type} name={name} onChange={onChangeHandle} defaultValue={val} placeholder="Enter here" />
                        : <FormControl componentClass={type} name={name} onChange={onChangeHandle} defaultValue={val} placeholder="Enter here" />
                    }
                </Col>
            </FormGroup>
        );
    }

    const handleChange = (e) => {
        console.log(e.target);
        console.log(e.target.name);
        console.log(e.target.value);
        word[e.target.name] = e.target.value;
        setWord(word);
        setResult('');
    }  

    console.log(name);
    console.log(location.state.word);
    return (
        <Fragment>
        <Form horizontal onSubmit={this.CallUpdateApi} >
            <h3 >Edit Word: {word.name}</h3>
                <FormField type='text' label="Word Name" name="name" onChangeHandle={handleChange}  val={word.name} ></FormField>
                <FormField type='text' label="Pronounciation" name="pronounciation" onChangeHandle={handleChange}  val={word.pronounciation} ></FormField>
                <FormField type='text' label="Frequency" name="frequency" onChangeHandle={handleChange} val={word.frequency} ></FormField>
                <FormField type='textarea' label="Meaning Short" name="meaningShort" onChangeHandle={handleChange}  val={word.meaningShort} ></FormField>
                <FormField type='textarea' label="Meaning Long" name="meaningLong" onChangeHandle={handleChange} val={word.meaningLong} ></FormField>
                <FormField type='text' label="Meaning Other" name="meaningOther" onChangeHandle={handleChange}  val={word.meaningOther} ></FormField>
                <label>{result}</label>
            <FormGroup>
                <Col smOffset={2} sm={8}>
                    <button className="primary" onClick={CallUpdateApi}>Update</button>
                    <button className="primary" onClick={CallDeleteApi}>Delete</button>
                </Col>
            </FormGroup>
        </Form>
        </Fragment>
    );
    

}


export default EditWord;
