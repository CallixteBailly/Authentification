import * as React from 'react';
import { Formik, Form, Field, ErrorMessage, FormikHelpers } from "formik";
import * as Yup from "yup";
import { loginUser } from '../api/AuthentificationService';
import '../utils/login.css'
import { IonIcon } from '@ionic/react';
import { lockClosedOutline, mailOutline } from 'ionicons/icons';
import { useHistory } from 'react-router-dom';
import { useQueryClient } from 'react-query';

interface Login {
    password: string;
    email: string;
}

const loginSchema = Yup.object().shape({
    email: Yup.string().email('Invalid email address').required('Email is required'),
    password: Yup.string().min(8, 'Password must be at least 8 characters').required('Password is required')
});

const Login: React.FC = () => {
    const queryClient = useQueryClient();
    const history = useHistory();
    const handleSubmit = async (login: Login) => {
        await loginUser(login);
        queryClient.clear();
        history.push('/home')
    };

    return (
        <>
            <style>
                {`
                    body {
                        background-repeat: no-repeat;
                        background-size: cover;
                        background-position: center;
                        transition: background-image 2s ease-in-out;
                    }
                `}
            </style>
            <Formik
                initialValues={{ email: "", password: "" }}
                validationSchema={loginSchema}
                onSubmit={handleSubmit}
            >
                {({ isSubmitting }) => (
                    <Form>
                        <section>
                            <div className="form-box">
                                <div className="form-value">
                                    <h2>Signup</h2>
                                    <div className="inputbox">
                                        <IonIcon icon={mailOutline} />
                                        <Field type="email" name="email" required />
                                        <label>Email</label>
                                    </div>
                                    <ErrorMessage name="email" className='validation-message' component="div" />
                                    <div className="inputbox">
                                        <IonIcon icon={lockClosedOutline} />
                                        <Field type="password" name="password" required />
                                        <label>Password</label>
                                    </div>
                                    <ErrorMessage name="password" className='validation-message' component="div" />
                                    <div className="forget">
                                        <label><input type="checkbox" />Remember Me  <a href="#">Forget Password</a></label>
                                    </div>
                                    <button type="submit" disabled={isSubmitting}>
                                        Submit
                                    </button>
                                    <div className="register">
                                        <p>Don't have a account <a href="#">Register</a></p>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </Form>
                )}
            </Formik>
        </>
    );
};

export default Login;


