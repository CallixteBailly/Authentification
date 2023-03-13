import { useState, useEffect } from "react";
import { useQuery, useQueryClient } from "react-query";
import { NavLink, useHistory } from "react-router-dom";
import { getIsAuthenticated } from "../api/AuthentificationService";

const Home: React.FC = () => {
    const { data: user } = useQuery('authenticated', getIsAuthenticated);
    const queryClient = useQueryClient();
    const history = useHistory();

    const handleSubmit = async () => {
        localStorage.clear();
        await queryClient.clear();
        history.push("/");
    };

    return (
        <>
            <section>
                {user?.isAuthenticated ? (
                    <div className="home">
                        <h1>You are connected</h1>

                        <div>
                            <label>Token </label>
                            <textarea value={user?.token} readOnly />
                        </div>

                        <div>
                            <label>FirstName </label>
                            <input
                                className="label-to-input"
                                placeholder={user?.firstName}
                                readOnly
                            />
                        </div>

                        <div className="">
                            <label>LastName </label>
                            <input
                                className="label-to-input"
                                placeholder={user?.lastName}
                                readOnly
                            />
                        </div>

                        <div>
                            <button onClick={handleSubmit}>log out</button>
                        </div>
                    </div>
                ) : (
                    <NavLink className="nav-link" to="/">
                        You are not connected click here
                    </NavLink>
                )}
            </section>
        </>
    );
};

export default Home;
